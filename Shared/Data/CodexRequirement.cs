using System;

namespace Shared.Data
{
    /// <summary>
    /// Utility helpers to encode Codex requirements that may carry an additional transcendence stage.
    /// </summary>
    public static class CodexRequirement
    {
        // Reserve low 24 bits for the item index (supports up to ~16.7M items) and high 8 bits for the stage.
        public const int StageShift = 24;
        public const int StageMask = 0xFF;
        public const int IndexMask = (1 << StageShift) - 1;
        public const sbyte AnyStage = -1;
        private const sbyte EncodedAnyStage = unchecked((sbyte)StageMask); // 255 => Any

        /// <summary>
        /// Encodes an item index and optional stage into a single integer for storage.
        /// Stage -1 (Any) is used when no stage constraint exists.
        /// </summary>
        public static int Encode(int itemIndex, sbyte stage)
        {
            if (itemIndex < 0) throw new ArgumentOutOfRangeException(nameof(itemIndex));

            int safeIndex = itemIndex & IndexMask;
            int stageByte = stage == AnyStage ? EncodedAnyStage : stage & StageMask;
            return (stageByte << StageShift) | safeIndex;
        }

        /// <summary>
        /// Extracts the raw item index from an encoded requirement.
        /// </summary>
        public static int DecodeItemIndex(int encoded) => encoded & IndexMask;

        /// <summary>
        /// Extracts the stage requirement from an encoded requirement.
        /// Returns -1 when the requirement accepts any stage.
        /// </summary>
        public static sbyte DecodeStage(int encoded)
        {
            int raw = (encoded >> StageShift) & StageMask;
            return raw == StageMask ? AnyStage : (sbyte)raw;
        }

        /// <summary>
        /// Returns true if the provided stage value satisfies the encoded requirement.
        /// </summary>
        public static bool StageMatches(int encodedRequirement, sbyte actualStage)
        {
            sbyte requiredStage = DecodeStage(encodedRequirement);
            return requiredStage == AnyStage || actualStage == requiredStage;
        }
    }
}

