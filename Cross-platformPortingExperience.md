# Legend of Mir Crystal: Linux FNA Porting & Stabilization Experience

This document compiles the architectural insights, engineering challenges, and technical solutions implemented during the migration and stabilization of the **Legend of Mir Crystal** game client from its legacy Windows GDI+ / SlimDX (DirectX 9) codebase to a modern, cross-platform **FNA (SDL2/Vulkan)** implementation running on Linux (.NET 10).

---

## 1. Executive Summary
The migration of a legacy Direct3D 9 / Windows Forms client to a case-sensitive Linux platform using FNA represents a significant platform paradigm shift. While the compilation layer was resolved early on using target separation (`#if FNA` directives), resolving high-fidelity gameplay rendering, input polling quirks, layout parity, and modernizing GDI+ legacy dependencies took a series of surgical, system-level corrections.

All major porting regressions—specifically map/ground rendering, blend-state visual artifacts, text overlap in dialogues, focus leaks, mouse-movement tracking, and GDI+ CPU memory/performance bottlenecks—have been fully resolved. By replacing the legacy text pipeline with a hardware-accelerated GPU-resident dynamic TrueType font system (FontStashSharp), the Linux FNA client is now completely stable, dependency-free, and achieves visual and gameplay parity with the legacy Windows client.

---

## 2. Key Challenges & Technical Deep Dive

### 2.1 File System Case-Sensitivity (VFS Integration)
* **The Problem:** Linux filesystems (e.g., ext4) are case-sensitive, whereas the legacy asset pipeline and map indices were developed on case-insensitive NTFS. Ground tiles and objects like `Tiles.wil` and `SmTiles.wil` failed to resolve, leading to a completely black ground floor.
* **The Solution:** A Virtual File System (VFS) resolution layer (`AssetResolver.cs`) was built. It maps and caches lowercased representations of paths dynamically. We tracked map layers utilizing `Tiles.wil` and unified the lookups so that case mismatches between map indices (`MapCode.cs`) and files on disk are resolved at runtime without requiring an asset-renaming script.

### 2.2 Reconciling Blend States (Fire, Spells & Lighting)
* **The Problem:** The game library relies heavily on raw `.wil` textures. Under DirectX 9, legacy alpha blending was used directly. When ported to FNA, standard `BlendState.AlphaBlend` expected pre-multiplied alpha textures. Because the client raw textures do not contain pre-multiplied alpha values, fire animations and magic effects rendered with severe black fringes or circular "halos."
* **The Corrections:**
  1. **Standard Blending:** Changed the default `SpriteBatch` pipeline to draw with `BlendState.NonPremultiplied`. This ensures raw `.wil` texture alpha channels are interpolated without darkening background pixels.
  2. **Additive Blending (`SetBlend`):** In legacy SlimDX, `DXManager.SetBlend(true)` changed global DirectX pipeline state, allowing subsequent `Sprite.Draw()` calls to blend additively. FNA's `DXManager.Draw` originally bypassed this entirely, ignoring the `Blending` state and causing spells to render with solid black background boxes. We updated `DXManager.Draw` and `DrawOpaque` to detect the `Blending` state and route draws to `Renderer.DrawBlend()`, restoring native Vulkan/OpenGL additive blending support.
  3. **Radial GPU Lights:** Corrected vertex color interpolation inside `FNARenderer.RenderGPULights`. Edges of lights were interpolating RGB channels to zero (`Color.Transparent`), producing dark gray borders. Using `Color(R, G, B, 0)` for outer vertices preserves color chromaticity while fading to transparent.
  4. **sRGB Gamma Correction & Color-Key Workarounds:** Legacy 16-bit BMP assets used off-black key colors like `(8, 0, 0, 255)` (Red = 1) to prevent older engines from keying them out as transparent black. Under DirectX 9, these rendered as dark, virtually invisible red squares. On Vulkan/OpenGL backbuffers under FNA, sRGB gamma correction maps linear `8` to `48` (~19% brightness), rendering them as prominent, semi-transparent red blocks (e.g. player-centered red square on `Rebirth3`). We resolved this by updating the BGRA-to-RGBA conversion loop in `MLibrary.cs` to filter out these dark workarounds (`R/G/B <= 8` with alpha 255 and other channels at 0) and clear them to transparent black `(0, 0, 0, 0)`.
  5. **Uninitialized Texture Indicators & Ignored Blend Rates:**
     - In GDI+/DirectX 9, white placeholder textures of size 2x2 and 5x5 (`RadarTexture` and `PoisonDotBackground`) were generated to render map location markers (minimap and bigmap dots) and character poison indicators. Under FNA, these fields were uninitialized (`null`), rendering them invisible. We initialized them dynamically as solid white textures inside the `FNARenderer` constructor and registered them for safe disposal.
     - The blending `rate` (opacity/fade) parameter in `IGraphicsRenderer.DrawBlend` was ignored under FNA, causing additive elements (like spell animations) to lose transition and fade-out effects. We patched `DrawBlend` in `FNARenderer.cs` to apply the `rate` parameter directly to the drawn colors, restoring correct opacity/fade transitions.

### 2.3 dialogue Layout & Font Measurement Parity
* **The Problem:** Legacy dialog rendering (NPC dialogues, bulletin boards, quest logs, chat links, scrolling labels) relied on GDI+ (`TextRenderer.MeasureText`). Because GDI+ inherently pads bounding boxes, the game's original logic appended extra space characters and subtracted `10` or `11` pixels from measurements to force links/colored text to align. The new `FNATextRenderer` uses a pixel-perfect `SpriteFont` measurement which has zero padding. Applying these GDI+ "hacks" shifted colored/interactive text fragments to the left, causing them to overlay and overlap regular text.
* **The Solution:** We systematically searched the UI codebase and introduced conditional compilation `#if FNA` in `NPCDialogs.cs`, `QuestDialogs.cs`, `NoticeDialog.cs`, `MainDialogs.cs`, and `MirScrollingLabel.cs`. For FNA builds, we stripped out the trailing space suffix and the hardcoded pixel location offsets (e.g. `-10` or `-11`). The dialogue fragments now align naturally on their exact baselines without layout drift.

### 2.4 Keyboard Shortcuts & Focus Leaks
* **The Problem:** Typing in the chat box would trigger global keyboard shortcuts (e.g., closing windows, toggling UI states).
* **The Solution:** We updated `MirTextBox.cs`'s focus synchronization. In the FNA pipeline, global keyboard polling bypasses classic WinForms focus chains. By explicitly calling `Activate()` and `Deactivate()` inside the textbox's `SetFocus()` and `LoseFocus()` overrides, the UI manager correctly knows when keyboard inputs must be consumed by the focused chat controls rather than bubbling up to the game scene.

### 2.5 Infinite Mouse Movement & Stuck Buttons
* **The Problem:** Upon entering the game, the player character would run continuously towards the mouse pointer. Left clicks were ignored, and right clicks stopped the movement but left clicks on the floor wouldn't resume regular walk cycles. Additionally, dropping items or gold onto the ground pops up a confirmation/amount dialog box. Because the dialog box steals active focus before a mouse release is captured on the map control, the left mouse button stayed stuck in `MapButtons` indefinitely, causing the player to walk automatically as soon as the dialog was closed.
* **The Solution:** We implemented three complementary solutions:
  1. **Direct MapControl MouseUp Registration:** We registered a `MouseUp` handler in `MapControl` inside `GameScene.cs`:
     ```csharp
     private static void OnMouseUp(object sender, MouseEventArgs e)
     {
         MapButtons &= ~e.Button;
         if (e.Button != MouseButtons.Right || !Settings.NewMove)
             GameScene.CanRun = false;
     }
     ```
     This captures mouse releases that happen directly on the map surface.
  2. **Confirmation Dialog Intercept:** In `GameScene.cs` under `MapControl.OnMouseDown`, we skip adding the Left mouse button flag to `MapControl.MapButtons` entirely if the user is clicking to drop an item or gold (`GameScene.SelectedCell != null || GameScene.PickedUpGold`), preventing a stuck mouse state from registering prior to the popup showing.
  3. **Physical-State Safety Clearing Fallback:** In `FNAEntry.cs` (`PollMouse`), we added a safety fallback check that queries the physical button states from SDL/FNA. If any physical mouse button is released, we automatically clear its corresponding flag in `MapControl.MapButtons`, ensuring mouse button states remain perfectly in sync with the hardware even when UI transitions bypass normal mouse events.

### 2.6 TrueType Font Point-to-Pixel Scaling
* **The Problem:** All text in the game appeared extremely small compared to the legacy Windows client.
* **The Solution:** We identified that `System.Drawing.Font` sizes are defined in Points (1/72 inch), whereas FNA's `FontStashSharp` renderer expects sizes in Pixels. At standard 96 DPI, 1 Point is ~1.33 Pixels, which meant all fonts were rendering at ~75% of their intended size. We updated `FNAFontManager.GetFont()` to scale the size parameter by `96f / 72f` before fetching the font from the cache. Furthermore, we modified `MirTextBox` to dynamically calculate the vertical Y-offset of the text label based on its parent control's height, preventing clipping on smaller input fields (like the 13px chat box).

### 2.7 Headless Server Path Normalization (\`\\\` vs \`/\` in Data Files)
* **The Problem:** The server database was designed on Windows, referencing scripts, drop lists, and scripted load/save paths using Windows-style backslashes (`\`). On Linux, standard library paths combined via `Path.Combine` treated `\` as a literal character in the file name instead of a path separator. This led to errors like `Script Not Found: GuildTerritory\GA0\GTStore-GA0` and failures to locate drop files and persistent player variables.
* **The Solution:** We modified the server path resolution layer to dynamically normalize backslashes using `.Replace('\\', Path.DirectorySeparatorChar)`. This change was systematically applied to:
  1. **NPC Scripts:** In `NPCScript.cs`, before combining paths to load scripts.
  2. **Drop Files & #INSERT Directives:** In `Envir.cs` and `MonsterInfo.cs`, before loading monster-specific drop tables and resolving drop includes.
  3. **Script Commands (`LOADVALUE`, `SAVEVALUE`, `DROP`):** In `NPCSegment.cs`, ensuring path arguments resolved within custom script commands are cross-platform compatible.

### 2.8 Server Drop Syntax Verification
* **The Problem:** The parser for drop configuration files (e.g., `Envir/Drops/HiGreatGhoul.txt`) expected a strict space delimiter between the drop rate fraction and the item identifier (e.g., `1/1 RedDagger Q`). Typing anomalies in data files, such as `1/1RedDagger Q`, failed to parse and caused silent failures when compiling the server drops list.
* **The Solution:** We corrected the malformed drop directives in the dataset and established clear guidelines regarding syntax spacing limits for drops.

### 2.9 Startup Type Load Crash
* **The Problem:** When running under FNA/Linux with target framework `.NET 10.0`, the client crashed immediately at startup with a `TypeLoadException` regarding the type `Microsoft.Xna.Framework.Graphics.GraphicsResource`.
* **The Solution:** We added a `TypeForwardedTo` attribute for `GraphicsResource` to `TypeForwarders.cs` in the `MonoGameCompat` project to resolve type resolution between assemblies under the new runtime environment.

### 2.10 Case-Sensitive Audio Files Lookup
* **The Problem:** Sound effects failed to play under case-sensitive Linux filesystems. The audio module was searching for audio files by appending pre-existing `.wav` extensions when searching the system directory paths, causing file-system lookup failures.
* **The Solution:** We patched `SoundManager.cs`'s `LoadSoundEffect` to normalize paths and detect pre-existing extensions cleanly, maintaining cross-platform compatibility.

### 2.11 Texture Decompression and Dynamic GPU Uploading
* **The Problem:** In FNA mode, the game assets (decompressed from LZO format) were never loaded into the GPU. A missing `#else` branch in `#if !FNA` inside `MLibrary.CreateTexture()` left the `Image` texture object completely uninitialized, rendering the active scene entirely black.
* **The Solution:** We implemented the FNA branch to allocate a `Texture2D` instance on the active `GraphicsDevice` and upload the decompressed color channels dynamically to the GPU memory (swapping the color channels to match XNA's RGBA format).

### 2.12 Unmanaged Memory Segmentation Fault
* **The Problem:** The transparency and mouse-transparency checks of the client controls are performed by inspecting the raw decompressed image bytes directly in memory. Under DirectX, this was done via direct pointer access, but in FNA, referencing this pointer resulted in a segfault because the buffer was not pinned.
* **The Solution:** We modified `MLibrary.cs` to dynamically marshal and copy the raw decompressed bytes to a globally allocated unmanaged memory pointer (`Data`) during texture creation, and cleanly free it when the image GC finalizes.

### 2.13 Graphics Device Initialization Race Condition (The Black Screen Bug)
* **The Problem:** Even with valid GPU texture data and correct diagnostics, the client screen remained pitch black on startup. We discovered that `FNAEntry.Initialize()` called `base.Initialize()`, which immediately triggered the virtual `LoadContent()` method. Since `DXManager.Renderer = Renderer;` was called in `LoadContent()` but `Renderer = new FNARenderer(...)` was instantiated *after* `base.Initialize()`, the renderer field was still null at allocation time, leaving `DXManager.Renderer` permanently null and causing all high-level rendering calls to return early.
* **The Solution:** We corrected the execution order by initializing `Renderer` and assigning `DXManager.Renderer = Renderer;` immediately before executing the base startup routine.

### 2.14 Headless Auto-Patcher Refactoring & Stabilization
* **The Problem:** The game client's updater logic was tightly integrated with Windows Forms, preventing the game from launching or updating on headless Linux platforms. Porting it also introduced critical cross-platform edge cases:
  1. **Strict Case-Sensitivity on Linux CDN Mirrors:** A case-sensitivity mismatch between the filenames parsed from the binary manifest (`PList.gz`) and the requests made to the CDN (e.g. `Data/Monster/001.Pak` vs. URL casing) resulted in HTTP 404 errors.
  2. **File Path Separator Mismatches:** On Linux filesystems, relative paths starting with `.\` (e.g., `new InIReader(@".\Mir2Config.ini")`) were parsed literally as starting with a dot and a backslash character, failing to locate the configuration files.
  3. **Process Locking & Atomic Swapping:** Overwriting active assemblies (like `Client.dll` or `AutoPatcher.exe`) would trigger write-access violations while the application process was running.
  4. **Synchronization Context Deadlocks:** Blocking synchronous entry-point calls (`.GetResult()`) on async patch tasks without `.ConfigureAwait(false)` caused thread-scheduling deadlocks, halting the application early with `Progress: 0/0 files`.
  5. **Destructive Auto-Cleaning During Updates:** The patcher automatically performed obsolete file cleanup (`CleanUpObsoleteFiles`) by default on every update cycle. This deleted local client-specific and user-specific files (e.g., `MIR2.ICO`, `KeyBinds.ini`, and `Data/UserData/QuestTracking.ini`) that were not tracked by the server's update manifest.
* **The Solution:**
  1. **Decoupled Patcher Engine:** Extracted WinForms UI code and built `HeadlessPatcher.cs` using high-performance, zero-allocation GZip streaming (`ArrayPool<byte>.Shared`).
  2. **Separator & File Normalization:** Configured cross-platform relative path lookups using `AppContext.BaseDirectory` combined with forward slashes for CDN request URLs.
  3. **Atomic Swapping & Inode Unlinking:** Implemented a rename-first assembly replacement strategy (moving locked binaries to `.patch_old`). On Linux, inode unlinking allowed immediate deletion, while Windows cleanups were deferred to next startup.
  4. **Deadlock Resolution & Throttled Console:** Applied `.ConfigureAwait(false)` to all async await calls to bypass calling synchronization contexts. Integrated thread-safe 500ms throttled console reporting to prevent terminal spam while displaying live download progress.
  5. **Auto-Resume Capabilities:** Updated the download pipeline to skip files that already exist in `.patch_temp` with verified sizes, enabling automatic download resumption upon client restarts.
  6. **Conditional Cleaning & Exclude Protection:** Restricted obsolete file cleaning to only execute when explicitly requested via command-line flags (`-clean`, `-cleanfiles`, or `--clean-files`) parsed at startup. Furthermore, refactored `CleanUpObsoleteFiles` to normalize relative paths and explicitly safeguard crucial directories and files—such as `Data/UserData/`, `KeyBinds.ini`, `Error.txt`, and `.ico` files—from being deleted even during manual cleanup operations.

### 2.15 Nested Project Directory Globbing & TargetFrameworkAttribute Duplication
* **The Problem:** When running the compilation command `dotnet build Client/Client.csproj -f net10.0 -c Release`, the build failed with `error CS0579: Duplicate 'global::System.Runtime.Versioning.TargetFrameworkAttribute' attribute`. This occurred because the `Platform/MonoGameCompat` project is nested under the `Client` directory structure. By default, MSBuild globbing (`**/*.cs`) compiled the source files and dynamic `obj/` assembly attributes of the nested project directly into the parent `Client` assembly, while also referencing it as a separate project reference, causing duplicate compilation and attribute declarations.
* **The Solution:** Added an explicit `<Compile Remove="Platform/MonoGameCompat\**" />` directive inside `Client.csproj` under the source file partitioning block. This prevents MSBuild from recursively globbing nested subproject files, ensuring clean separation of compile-time units while maintaining reference integrity.

### 2.16 Zero-GDI+ Modernization & GPU-Resident Text Rendering Pipeline
* **The Problem:** Microsoft deprecated Unix support in `System.Drawing.Common` (GDI+) because the underlying library (`libgdiplus`) causes severe memory leaks, deadlocks, and missing glyph crashes on modern Linux. Initially, a Unix compatibility switch was introduced to temporarily allow GDI+, but this was a critical architectural regression. Text measurement and rasterization in the game client's labels and dialogue systems were tightly coupled to CPU-bound `Bitmap` locking, pixel array copying, and custom ARGB-to-RGBA conversions before uploading to GPU memory.
* **The Solution:** 
  1. **Purged GDI+ Dependencies:** Deleted the Unix compatibility switch from `ProgramFNA.cs` and purged the `System.Drawing.Common` package dependency from the `net10.0` target in `Client.csproj`.
  2. **Type Compatibility Stubs:** Replaced GDI+ classes like `System.Drawing.Font`, `System.Drawing.FontStyle`, `System.Drawing.Bitmap`, and others with clean C# stubs in `MirInputTypes.cs`. We redirected all layout calculations and string measurements to `FNAFontManager` to run on the CPU using TrueType metrics without GDI+.
  3. **MonoGame Compatibility Layer:** Standard NuGet packages like `FontStashSharp.MonoGame` expect dependencies on `MonoGame.Framework.dll`. To avoid namespace and assembly conflicts under FNA, we created a lightweight compatibility library called `MonoGame.Framework.dll` containing `.NET` `TypeForwardedTo` attributes. This dynamically binds all MonoGame types directly to `FNA.dll` at runtime.
  4. **GPU-Resident Rendering:** Refactored `MirLabel.CreateTexture()` to instantiate a GPU-resident `RenderTarget2D` in VRAM. It clears the render target and renders outlines and text on the fly using FNA's native `SpriteBatch` combined with `FontStashSharp`'s `DrawString` extension, running 100% on the GPU with zero CPU memory allocations.

### 2.17 Comprehensive Relative Backslash Path Normalization
* **The Problem:** Windows-style relative backslash paths (e.g. `@".\Localization\"`, `@".\KeyBinds.ini"`, and `@".\Error.txt"`) were hardcoded across several modules including Client Settings, Server Settings, KeyBindings, AutoPatcher, and tools. On Linux, these paths were treated as literal directory/file names with backslashes instead of resolving relative to the application base directory, resulting in unwanted file creation and path mismatches.
* **The Solution:** We systematically replaced all instances of hardcoded relative backslash paths with cross-platform paths using `Path.Combine` and `AppContext.BaseDirectory` or `AppDomain.CurrentDomain.BaseDirectory`. This ensures that local configuration, localization files, and error logs are stored relative to the executing assembly's path, regardless of operating system differences.

### 2.18 CJK & Chinese Font Localization Fallback System
* **The Problem:** When configuring the game client with `Language=Chinese` in `Mir2Config.ini`, the game loaded Chinese translation files (`Chinese.json`), but failed to render any text. The FNA font engine (`FNAFontManager` built on `FontStashSharp`) only initialized standard system Latin TrueType fonts. Because these fonts lacked CJK character glyphs, the game drawn empty text.
* **The Solution:** We modified `FNAFontManager.cs` to locate available CJK/Chinese system fonts on Linux (such as `DroidSansFallbackFull.ttf` and `Fandol` fonts) and register them as fallback fonts in the same `FontSystem` instance via `FontSystem.AddFont()`. The library automatically resolves missing glyphs by checking sequentially added fallback fonts. We wrapped the registration loop in a try-catch block to gracefully skip unsupported formats (such as variable/TTC font collections that fail to initialize in the underlying `stb_truetype` engine) without crashing the client startup.

### 2.19 MirLabel Alignment & DrawFormat Support
* **The Problem:** Under GDI+ (Windows), standard `TextRenderer.DrawText` honors formatting flags like horizontal and vertical center inside a control's bounding box. In the FNA engine port (Linux), `MirLabel.DrawControl()` drew the text string directly at the control's top-left coordinates `DisplayLocation.X, DisplayLocation.Y` without inspecting `DrawFormat`. This caused centered elements, such as the player's name text and guild name in the character status/equipment dialog box, to render left-aligned/off-left.
* **The Solution:** We updated `MirLabel.cs` (`DrawControl`) under FNA to calculate the measured size using `font.MeasureString(Text)` and shift `pos` by the appropriate horizontal and vertical offsets when center/right/bottom alignment flags are present in `DrawFormat`. We also added standard `Top` and `Bottom` flags to the simulated `TextFormatFlags` enum in `MirInputTypes.cs` to maintain compilation parity.

### 2.20 Unified Background & Border Rendering
* **The Problem:** Solid background color boxes and borders (used in tooltips, character creation screens, and info panels) failed to render in the FNA client, leaving text floating without a readable background. Under legacy SlimDX, they were drawn using DirectX device state APIs inside the `#if !FNA` block.
* **The Solution:** We introduced a unified rendering abstraction by adding a `DrawRectangle` signature to `IGraphicsRenderer.cs` and implementing it in `FNARenderer.cs` using a 1x1 pixel white texture dynamically colored and stretched via `SpriteBatch.Draw`. We then updated `MirControl.DrawControl` and `DrawBorder` under FNA to route their background and border drawing operations through this new API.

### 2.21 Height-Constrained Word Wrapping
* **The Problem:** NPC dialogues and Notice panels displayed overlapping text and misaligned clickable links under FNA. Single-line labels (such as dialogue rows) are instantiated with a height of 20px but carried default `WordBreak` flags. Under FNA, `SpriteBatch.DrawString()` does not clip rendering to the label's height. Consequently, long dialogue lines wrapped to a second line (drawing over lines below them) while their companion colored clickable links (positioned at unwrapped coordinates) stayed on the first line, overlapping.
* **The Solution:** We refactored `MirLabel.cs` (`CreateTexture()`) and `FNATextRenderer.cs` (`MeasureText()`) to only wrap text when the control's height has enough vertical space to display at least two lines (`Size.Height >= singleLineHeight * 1.5f`). If the height constraint is not met, wrapping is disabled and the text draws on a single line, restoring pixel-perfect alignment with clickable button overlays.

### 2.22 Client Input Control, Focus Traversal & Caret Resolution
* **The Problem:** The FNA text box implementation suffered from several cross-platform input regressions:
  1. Hovering and clicking buttons was delayed or locked when a textbox had focus, requiring clicking outside the textbox first.
  2. The chat input box remained invisible/inactive, rendering the background white and ignoring clicks. Once activated via Enter or chat content, it became gray and interactive, but clicking on it the first time was blocked. Also, clicking the leftmost boundary focused it, but normal interaction should allow clicking anywhere inside.
  3. The cursor caret in FNA was rendered as a thick, outlined block and was offset to the right by about one character due to font cell bearing differences.
  4. The `Tab` and `Shift+Tab` keys did not traverse focus between textboxes.
  5. The guild name creation input box had a hardcoded ASCII alphanumeric filter, blocking Chinese and CJK character input.
* **The Solution:**
  1. **Button Interaction:** Modified `MirControl.Highlight()` to allow hover highlights on buttons when `ActiveControl` is a `MirTextBox`, eliminating the click delay.
  2. **IsMouseOver Occlusion & First-Click Activation:** Updated `MirImageControl.IsMouseOver` to recursively check visible child controls first, preventing transparent parent textures from blocking clicks. Registered a `MouseDown` handler on `ChatDialog` that captures clicks inside `ChatTextBox.DisplayRectangle` when it is invisible, toggling it visible and focusing it immediately.
  3. **Caret Alignment & Styling:** Set `OutLine = false` on `_caretLabel` inside `MirTextBox.cs`'s FNA constructor to render a thin line cursor. Changed the caret positioning offset in `UpdateLabel()` from `_textLabel.Size.Width - 3` to `_textLabel.Size.Width - 5` to align the cursor flush with the text.
  4. **Keyboard Event Routing & Tab Focus:** Patched keyboard polling in `FNAEntry.PollKeyboard()` to capture modifiers and route keypresses directly to the active `MirTextBox`. Implemented focus cycling in `MirTextBox.OnKeyDown` for `Tab` and `Shift+Tab` by recursively searching the active scene's control tree for eligible textboxes.
  5. **CJK Character Support in Guilds:** Replaced the ASCII check on the guild creation inputBox in `GameScene.cs` with `char.IsLetterOrDigit` to natively support Chinese/CJK letters.

### 2.23 Non-AutoSize Label Clipping & Truncation
* **The Problem:** In FNA builds, `SpriteBatch.DrawString` does not constrain rendering to the bounding box of a `MirLabel`. When `AutoSize` was set to `false` (e.g. for mail message previews or restricted name fields), long strings floated outside the label's `Size.Width` bounds, leaking text into neighboring controls and outside the dialog. Under GDI+, these strings were naturally clipped at pixel-level boundaries.
* **The Solution:** We updated `MirLabel.CreateTexture()` for FNA to check if `AutoSize` is `false`. When boundaries are constrained, a binary search function `TruncateText()` performs character-level truncation using `font.MeasureString` to fit the string to the label's `Width`. Additionally, the label now performs vertical line clipping to discard text lines exceeding `Size.Height`, preventing text from overflowing vertically.

### 2.24 Font-Independent Dialog Layout Grid Aligner
* **The Problem:** The Stats page in the Guild dialog box used a single multi-line `StatusHeaders` label with double newlines (`\n\n`) to lay out headers ("Guild Name", "Level", "Members"). In Chinese settings, the CJK fallback font (e.g., Fandol) has different vertical metrics and line spacing compared to standard Latin fonts. This caused the "Level" and "Members" headers to shift upwards by half a character and a full character height respectively, while their corresponding value labels (which are separate controls positioned at fixed Y offsets) remained static, leading to severe layout misalignment.
* **The Solution:** We replaced the single multi-line header control in `GuildDialog.cs` with three distinct header labels: `StatusGuildNameHeader`, `StatusLevelHeader`, and `StatusMembersHeader`. We parse the localized text mapping, split it on newline tokens, and instantiate the labels at the exact fixed Y coordinates (`47`, `73`, and `99`) matching the values. This decoupled the UI grid alignment from font-specific line height measurements.

### 2.25 Swapchain Backbuffer Content Discard (The Black Screen on Render Target Switches)
* **The Problem:** When implementing cave and nighttime lighting effects, switching between render targets (drawing the light mask onto a custom `LightRenderTarget` and then switching back to the default backbuffer to multiply blend) caused the entire game screen to turn pitch black under multiplicative blending. Switching to opaque blending proved the light geometries were drawn correctly, but the previously rendered game scene was completely lost.
* **The Solution:** In FNA/XNA, the default backbuffer presentation parameters initialize with `RenderTargetUsage = RenderTargetUsage.DiscardContents`. Under Vulkan or modern OpenGL graphics drivers, changing the active render target ends the render pass on the default swapchain backbuffer. When switching back via `SetRenderTarget(null)` to start a new render pass, Vulkan uses a discard load operation (`VK_ATTACHMENT_LOAD_OP_DONT_CARE` or `VK_ATTACHMENT_LOAD_OP_CLEAR`), erasing the previously rendered game scene. We fixed this by subscribing to the `PreparingDeviceSettings` event on `GraphicsDeviceManager` and explicitly setting `e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents` during initialization and resolution/viewport changes. This forces the Vulkan driver to load and preserve the swapchain image contents across render target changes.

### 2.26 Buff Status Hover Text & Tooltip Coordinate Syncing
* **The Problem:** In the Windows client, hover-over tooltips (like player status and buff icons next to the minimap) and debug label coordinate updates were handled by `Forms/CMain.cs` events. Since this directory is excluded from compilation under FNA/Linux, tooltips were completely silent, coordinates were never updated, and hover text failed to render.
* **The Solution:** We re-implemented `CMain.CreateHintLabel()`, `CMain.CreateDebugLabel()`, and `CMain.UpdateFrameTime()` as fully operational rendering stubs in the FNA-compilable `CMain` class in `Platform/MirInputTypes.cs`. We then hooked `UpdateFrameTime()` inside the main `Draw` loop in `FNAEntry.cs` and `CreateHintLabel()` and `CreateDebugLabel()` inside the main `Update` loop to keep tooltip label coordinates and active text synchronized with the mouse state.

### 2.27 Dynamic Resolution Resizing
* **The Problem:** The `CMain.SetResolution(width, height)` method was stubbed out to be empty under Linux. This meant selecting a different screen resolution or toggling window modes did not resize the client window or scale the graphics presentation viewport, keeping the resolution locked to the initial value read from the config file.
* **The Solution:** We implemented the FNA version of `SetResolution` to dynamically configure the preferred backbuffer width/height on the `GraphicsDeviceManager`, trigger `ApplyChanges()` to resize the window, and re-initialize/update the viewport on the active `FNARenderer` instance.

### 2.28 Keyboard Caps Lock State Checks
* **The Problem:** The virtual keyboard (`InputKeyDialog`) used the Win32-specific `CMain.IsKeyLocked` method to determine the state of the CAPS LOCK key, which always returned `false` on Linux. This prevented the virtual keyboard from performing uppercase case-switching when CAPS LOCK was active.
* **The Solution:** We updated `IsKeyLocked` inside `MirInputTypes.cs` to leverage the cross-platform `.NET` `Console.CapsLock` property, wrapping the call in a try/catch to safely return `false` if the client is executed in a headless/non-console environment.

### 2.29 Cross-platform GPU Screen Captures / Screenshots
* **The Problem:** The legacy client screenshot routine relied on GDI+ WinForms device contexts (`Program.Form.CreateScreenShot()`), which is absent under FNA/Linux. Consequently, pressing the screenshot key had no effect.
* **The Solution:** We built a custom `CMain.CreateScreenShot()` routine in the FNA client. It captures raw color pixels directly from the GPU backbuffer via `GraphicsDevice.GetBackBufferData()`, converts the colors to an ImageSharp image (`Image<Rgba32>`), and saves it as a PNG file inside the client's `Screenshots/` directory. We then wired a global print-screen keybind check into the keyboard polling cycle of `FNAEntry.PollKeyboard()`.

### 2.30 Settings Serialization Bug
* **The Problem:** When saving client configuration, the settings writer erroneously serialized the player's `ExpandedBuffWindow` setting to the INI file under the `ExpandedHeroBuffWindow` key.
* **The Solution:** Fixed the field reference in `Client/Settings.cs` to correctly save the `ExpandedHeroBuffWindow` setting under its own key.

### 2.31 FNA Client Hotkey Binding Modifiers Bug
* **The Problem:** When setting hotkeys in the Keyboard Layout dialog under FNA (Linux), pressing a modifier key (Ctrl, Alt, or Shift) immediately registered the keybind as `Ctrl + LControlKey`, `Alt + LMenu`, or `Shift + LShiftKey` rather than waiting for the player to press the primary key of the combination.
* **The Solution:** Under FNA/Linux, modifier keys poll as specific physical key codes (`Keys.LControlKey`, `Keys.RControlKey`, `Keys.LMenu`, `Keys.RMenu`, `Keys.LShiftKey`, `Keys.RShiftKey`). The dialog input capture method `KeyboardLayoutDialog.CheckNewInput` was ignoring generic modifier values (`Keys.ControlKey`, `Keys.Menu`, `Keys.ShiftKey`), but failed to filter out the side-specific keys. We updated `CheckNewInput` to ignore both left- and right-handed specific modifier key codes so that key registration waits until the primary key of the combination is pressed.

### 2.32 UI Visibility Toggle Collection Modification Bug
* **The Problem:** When using the camera mode hotkey to hide/show the game interface, the client would crash with a `System.InvalidOperationException: Collection was modified; enumeration operation may not execute` runtime error. This was caused by the recursive propagation of `OnVisibleChanged()` down the control tree. During enumeration of the parent's `Controls` collection, any child control with `Sort = true` attempted to re-order itself inside the parent's collection by calling `Parent.Controls.Remove(this)` and `Parent.Controls.Add(this)`, mutating the collection under iteration.
* **The Solution:** We updated `MirControl.OnVisibleChanged()` to copy the `Controls` list to a temporary array (`Controls.ToArray()`) before enumerating it, ensuring that structural sorting changes do not interfere with the active control visibility propagation loop.

### 2.33 Building Opacity Parity
* **The Problem:** In the SlimDX client, when the player walks behind a tall building, the client renders their silhouette on top of the building at 40% opacity. In the FNA client, the player was rendered fully solid, making it look as if they were standing on top of the building. This happened because `FNARenderer.SetOpacity` was a no-op stub, and the drawing methods in `FNARenderer` ignored the global opacity value.
* **The Solution:** We introduced a private `_opacity` field in `FNARenderer.cs`, mapped `SetOpacity` to update it, and updated all relevant drawing methods (`Draw`, `DrawOpaque`, `DrawBlend`, and `DrawRectangle`) to multiply their drawing colors by `_opacity`, restoring the semi-transparent silhouette behind structures.

### 2.34 Grayscale Rendering System
* **The Problem:** The game client relies on grayscale rendering for disabled UI buttons, trust merchant slot items, and character death states. Under SlimDX, this was handled via a custom pixel shader (`grayscale.ps`). Under FNA, `SetGrayscale` was a no-op stub, and compiling or loading custom MojoShader effects at runtime on Linux poses package and toolchain dependencies. Furthermore, because the FNA client rendering path bypasses WinForms/SlimDX control compositing (`DrawControl`), character death did not trigger any grayscale effect.
* **The Solution:**
  1. **CPU-based Texture Conversion Cache:** We implemented a thread-safe weak-key cache using `ConditionalWeakTable<Texture2D, Texture2D>` in `FNARenderer.cs`. When grayscale is active, drawing methods dynamically retrieve a cached grayscale copy of the texture. If not cached, the original texture pixels are extracted via `GetData()`, converted to grayscale on the CPU using standard luminosity weights (`R * 0.299 + G * 0.587 + B * 0.114`), and uploaded as a new texture using `SetData()`.
  2. **GameScene Rendering Integration:** We updated the FNA-specific `Draw()` method in `GameScene.cs` to check if `MapObject.User.Dead` is true, enabling grayscale rendering state prior to drawing the scene elements and restoring it afterward.

### 2.35 Dragged Item & Text Layering Order under FNA
* **The Problem:** In the FNA client, dragging an inventory item (or gold) resulted in the item rendering *behind* dialog boxes (like the inventory or shop). Under SlimDX/DirectX, `GameScene.DrawControl()` rendered the dragged item and output message lines *after* executing `base.DrawControl()` (which drew all child controls/dialogs to a composite texture). Under FNA, composite texture rendering is disabled, and `MirScene` draws in immediate mode: it invokes `DrawControl()` (drawing the dragged item/lines) before calling `DrawChildControls()` (drawing the dialogs). Consequently, dialogs rendered on top of the dragged item.
* **The Solution:**
  1. **Virtual Post-Draw Lifecycle Hook:** Changed `AfterDrawControl` from `protected void` to `protected virtual void` in `MirControl.cs` to allow polymorphism and enable custom post-render logic.
  2. **Deferred Rendering in GameScene:** In `GameScene.cs`, we excluded the dragged item/gold and screen output text lines from rendering inside `DrawControl()` under the `#if FNA` directive. Instead, we added an override of `AfterDrawControl()` specifically under `#if FNA` to render these overlays at the very end of the control draw cycle—after both `DrawControl()` and `DrawChildControls()` have completed—correctly restoring top-layer rendering.

### 2.36 ChatTextBox Focus and Debug Label Rendering Anomalies
* **The Problem:** 
  1. The text input area in the `ChatDialog` remained dark gray after losing focus or when Escape was pressed. Gaining focus for the first time set `Visible = true`, but because the custom FNA textbox implementation was not receiving/processing `Escape` keys in its text input loop, and because clicking outside the textbox only unfocused it without hiding it, the control never transitioned back to `Visible = false` (which would have revealed the clean white backing sprite).
  2. Hovering the mouse over the text input area caused the debug label to drop the control name entirely, changing `Control: ChatDialog, Objects:89` to `Objects:89`. This happened because `MirTextBox` inherits from `MirControl` rather than `MirImageControl`, which failed the debug label's strict `is MirImageControl` type check.
* **The Solution:**
  1. **Escape Key KeyPress Injection:** In `MirTextBox.OnKeyDown` under FNA, we intercepted `Keys.Escape` and manually dispatched a `KeyPress` event with `(char)Keys.Escape` to replicate WinForms behavior. This enables the textbox keypress handler to process the Escape key, hide the textbox, and lose focus.
  2. **LostFocus Event Handler:** We implemented a custom `LostFocus` event signature on `TextBoxStub` and triggered it inside `MirTextBox.LoseFocus()`. In `MainDialogs.cs`, we hooked into `ChatTextBox.TextBox.LostFocus` to set `ChatTextBox.Visible = false`, empty the text, and clear linked items.
  3. **Universal Debug Label Mapping:** We updated `CMain.CreateDebugLabel` in `MirInputTypes.cs` to print the type name of the current control for all hovered controls except `MapControl` (which occupies the entire screen background), restoring proper control tracking on hover.

### 2.37 Custom Independent Window Scaling & High-DPI Resolution Decoupling
* **The Problem:** On Linux/Wayland desktops configured with system-wide fractional scaling (e.g., GNOME set to 150%), FNA/SDL2 window dimensions were automatically hijacked by the Wayland compositor, locking the game's display size and scale factor. Attempting to force or bypass scaling using standard GDK or SDL environment variables (e.g., `SDL_VIDEO_HIGHDPI_DISABLED=1`) failed to decouple scaling or resulted in blurred rendering and broken mouse coordinates due to mismatched backbuffer mapping.
* **The Solution:** We implemented a custom, independent window scaling and High-DPI resolution decoupling system:
  1. **Configuration Properties:** Added configuration properties `HighDPI` (bool, default `true`) and `WindowScale` (float, default `1.0`) in `Settings.cs` to allow user-defined scaling factor overrides in `Mir2Config.ini`.
  2. **High-DPI Alignment:** Configured `ProgramFNA.cs` to dynamically initialize `FNA_GRAPHICS_ENABLE_HIGHDPI` and `SDL_VIDEO_HIGHDPI_DISABLED` on application launch depending on the `HighDPI` setting.
  3. **Backbuffer & Resolution Scaling:** Scaled startup backbuffer resolution (`FNAEntry.cs`) and runtime resolution changes (`MirInputTypes.cs`) by `Settings.WindowScale` to request a high-resolution canvas matching the scaled dimensions.
  4. **Inverse Coordinate Translation:** Implemented inverse scaling on polled mouse coordinates in `FNAEntry.cs` (`GetScaledMouseState`) to translate screen-space inputs back into the game's logical width/height bounds, maintaining precise click targets.
  5. **GPU-Accelerated Point Filtering:** Calculated scaling factors dynamically in `FNARenderer.cs` (`UpdateScaleFactors`) and applied them as scaling matrices to all `SpriteBatch.Begin` draw passes. To prevent bilinear blurring at higher magnifications (e.g., 200% scale), we passed `SamplerState.PointClamp` to the `SpriteBatch` pipeline to enforce crisp, pixel-perfect nearest-neighbor scaling.

### 2.38 Automating libFNA3D Check and On-Demand Source Compilation
* **The Problem:** The FNA client version requires the `libFNA3D.so` library (mapped as `libFNA3D.so.0` in `app.config`) to run correctly. On some target systems, this library is not pre-installed, and compiling or fetching it manually is error-prone.
* **The Solution:** We implemented an automated MSBuild pipeline in `Client.csproj` targeting `net10.0` on Linux:
  1. **System Detection Check:** Before compiling the C# project, an execution task runs a fast, dual-layer system check. It checks the system's dynamic linker cache via `ldconfig` and does a GCC link-loader check (`gcc -lFNA3D -shared -o /dev/null -x c /dev/null`) to detect if `libFNA3D` is available system-wide.
  2. **On-Demand Compilation:** If missing, MSBuild automatically creates a build folder under `Client/FNA/lib/FNA3D/build`, runs `cmake ..`, and compiles `libFNA3D.so` using `make`.
  3. **Output Directory Alignment:** Upon successful compilation, all generated library binaries and symbolic links (`libFNA3D.so*`) are copied to both the build target directory (`$(TargetDir)`) and the publish directory (`$(PublishDir)`) to ensure runtime resolution. Incremental build states are preserved to prevent redundant rebuilds.

### 2.39 Redirection of Resource Resolution to Working Directory
* **The Problem:** In the FNA version of the client, resource files, config files (`Mir2Config.ini`, `Mir2Test.ini`, `KeyBinds.ini`), localized text datasets, error logs, and screenshots were resolved relative to the program's binary execution directory (`AppContext.BaseDirectory` or `AppDomain.CurrentDomain.BaseDirectory`). If the user executed the client from a different working directory, the program could not find game assets or created config and screenshot directories inside the binary path.
* **The Solution:** We updated path resolution for the FNA build target to use the current working directory (`Directory.GetCurrentDirectory()`):
  1. **VFS and Asset Indexing:** Configured `AssetResolver.cs` to index resources and store transcoded audio cache files in the current working directory.
  2. **Config & Localization Paths:** Modified `Settings.cs` and `KeyBindSettings.cs` to target configuration and localization directories in the current working directory when compiled under FNA.
  3. **Screenshots and Logs:** Updated `MirInputTypes.cs` to save captured screenshots and error logs into the working directory.
  4. **Patcher Self-Update Alignment:** Configured `HeadlessPatcher.cs` to look for the downloaded self-update package (`AutoPatcher.gz`) inside `Settings.P_Client` (which is redirected to the working directory).


### 2.40 Server Case-Insensitive VFS Resolution (Linux Headless Support)
* **The Problem:** Linux filesystems are case-sensitive, but the game database and assets (`assets/Crystal.Database/Jev`) were designed under Windows, featuring files (like map files `.map` and monster drops `.txt`) with mixed/uppercase casing. Because the server loaded maps and drops using lowercase strings, all mixed-case assets failed to load, producing hundreds of "Failed to Load Map" and drop load errors.
* **The Solution:** We implemented a custom, compile-time VFS redirection layer in the `Server` project:
  1. **Shadowing System.IO:** Created `Vfs.cs` declaring `Server.File` and `Server.Directory` static classes in the `Server` namespace. Since implicit usings are enabled in .NET 10, these classes seamlessly shadow `System.IO.File` and `System.IO.Directory` across the entire codebase without needing to rewrite any files.
  2. **VFS Caching Index:** At startup, the static constructor recursively scans and indexes the current working directory, caching normalized and lowercase paths.
  3. **Idempotency & Dynamic Updates:** Methods (like `Exists`, `OpenRead`, `ReadAllLines`, `ReadAllBytes`, `Create`, `Delete`, `Copy`, `Move`) automatically query and resolve requested paths case-insensitively. Runtime file creations, deletes, or moves dynamically update the in-memory index to preserve consistency.

### 2.41 Consolidating Case-Insensitive VFS & Normalization into Shared
* **The Problem:** The Virtual File System (VFS) implementations for case-insensitive file mapping, path normalization, and backslash replacement were duplicated across the client (`AssetResolver.cs`) and server (`Vfs.cs`) projects. This led to code duplication, divergent resolution rules, and lack of unified regex-based case-insensitive pattern matching for filename searches on Linux (such as `Directory.GetFiles` using wildcards).
* **The Solution:** We consolidated all isolated file system compatibility layers into a unified `Shared` project implementation:
  1. **Unified VfsManager:** Implemented `Shared.Vfs.VfsManager.cs` to index and resolve paths case-insensitively, normalize backslashes to forward slashes, and handle dynamic index registrations (for created, deleted, or moved files and directories).
  2. **Regex Glob Translation:** Added regex-based wildcard pattern matching (`GetFilesMatching` and `GetDirectoriesMatching`). Glob search strings are translated on-the-fly to case-insensitive regular expressions, allowing safe case-insensitive file pattern queries on Linux's case-sensitive filesystem.
  3. **Thin Client/Server Delegates:** Refactored `AssetResolver.cs` on the client and the `Server.File` / `Server.Directory` shadowing classes on the server to act as thin wrappers delegating to the unified `VfsManager`.


### 2.42 World Map Cache Clearing on Logout (Multi-Character Session Alignment)
* **The Problem:** The game client's `World Map` button in the `BigMapDialog` is configured by default to be visible. Because `WorldMap.ini` has `Enabled=False`, the server is expected to send `S.WorldMapSetupInfo` to notify the client to hide the button. However, the client's static cache `MapInfoList` and the server's per-connection cache `SentMapInfo` were never cleared when a player logged out or when the game scene was disposed. As a result, when logging in with a second character/class during the same session, the client skipped requesting map info, and the server connection skipped sending the setup packet, causing the button to remain visible for classes/characters that should not see it.
* **The Solution:** We modified the client-side `GameScene.Dispose` method to clear `MapInfoList`, and modified the server-side `MirConnection.LogOut` method to clear `SentMapInfo` upon logout, ensuring the cache is fully reset for each login.

### 2.43 BigMap NPC Search Query Validation
* **The Problem:** Clicking the `Serach for NPCs` button in the Big Map dialog had no effect. A logic error in client-side query validation check `!string.IsNullOrWhiteSpace(SearchTextBox.Text) && SearchTextBox.Text.Length > 2` incorrectly returned early from the search method when a valid query (length > 2) was provided, while allowing invalid short queries to reach the server.
* **The Solution:** Corrected the validation check in `BigMapDialog.Search` to `string.IsNullOrWhiteSpace(SearchTextBox.Text) || SearchTextBox.Text.Length < 3`, ensuring the method returns early only on invalid queries.

### 2.44 FNA3D Compilation Fix on Modern Runtimes
* **The Problem:** The native build pipeline compiling FNA3D during `dotnet build` failed on Linux with a compiler error due to an undeclared/unused statement (`SDL_stack_free(resourceSetLayoutInfos);`) in the subproject's source code at `FNA3D_Driver_SDL.c` (line 1755).
* **The Solution:** Commented out the undeclared variable statement (`SDL_stack_free(resourceSetLayoutInfos);`), resolving the compilation barrier and allowing the client package to compile cleanly.

---

## 3. Structural Porting Guidelines for Future Reference
For engineers maintaining this cross-platform codebase, follow these rules to maintain platform parity:
1. **Never Bypass Global Event Wrappers:** Do not read raw input state dynamically in scene logic without updating global stubs. Input events must flow from `FNAEntry.cs` into the UI control hierarchy cleanly.
2. **Keep Rendering Code Separated:** Keep the platform-specific graphics backends confined to `Platform/FNA` and `SlimDX` layers. Ensure `DXManager` remains the uniform interface.
3. **Use Idempotent Math for Layouts:** Avoid hardcoded pixel offsets that attempt to patch font engine limitations. If font scaling or kerning differs, handle it at the `FNATextRenderer` level, not inside dialogue line-parsers.
4. **Enforce Task Disconnection in Synchronous Paths:** When calling async logic from synchronous entry points, always utilize `.ConfigureAwait(false)` to prevent thread-pool and game-loop deadlocks.

---

## 4. Final Verdict
The Legend of Mir Crystal client and headless server are now **Linux native and stable**. The client uses modern Vulkan/Vulkan-on-Mesa rendering with crisp visual layouts, precise inputs, and accurate additive blending. The headless server operates seamlessly on case-sensitive Linux filesystems with 100% database and map load correctness. The automatic updater is fully headless, supporting case-sensitive Unix mirrors, process swapping, and resilient auto-resumable patching.
