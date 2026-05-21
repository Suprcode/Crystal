global using System.Drawing;
global using Keys = Client.Platform.MirKeys;
global using MouseButtons = Client.Platform.MirMouseButtons;
global using MouseEventArgs = Client.Platform.MirMouseEventArgs;
global using KeyEventArgs = Client.Platform.MirKeyEventArgs;
global using KeyPressEventArgs = Client.Platform.MirKeyPressEventArgs;
global using MouseEventHandler = Client.Platform.MirMouseEventHandler;
global using KeyEventHandler = Client.Platform.MirKeyEventHandler;
global using KeyPressEventHandler = Client.Platform.MirKeyPressEventHandler;

#if FNA
global using Vector2 = Microsoft.Xna.Framework.Vector2;
global using Vector3 = Microsoft.Xna.Framework.Vector3;
global using Matrix = Microsoft.Xna.Framework.Matrix;
global using Texture = Microsoft.Xna.Framework.Graphics.Texture2D;
global using TextFormatFlags = System.Windows.Forms.TextFormatFlags;
global using TextRenderer = Client.Platform.FNA.FNATextRenderer;
global using SystemInformation = System.Windows.Forms.SystemInformation;
#endif
