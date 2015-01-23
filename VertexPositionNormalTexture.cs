// This definition doesn't exist in SharpDX for some reason so I've added it

using System;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Project
{
    /// <summary>
    /// Describes a custom vertex format structure that contains position and texture information.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPositionNormalTexture : IEquatable<VertexPositionNormalTexture>
    {
        /// <summary>
        /// Initializes a new <see cref="VertexPositionNormalTexture"/> instance.
        /// </summary>
        /// <param name="position">The position of this vertex.</param>
        /// <param name="normal">The vertex normal.</param>
        /// <param name="texture">The texture of this vertex.</param>
        public VertexPositionNormalTexture(Vector3 position, Vector3 normal, Vector2 textureCoordinate)
            : this()
        {
            Position = position;
            Normal = normal;
            TextureCoordinate = textureCoordinate;
        }

        /// <summary>
        /// XYZ position.
        /// </summary>
        [VertexElement("SV_POSITION")]
        public Vector3 Position;

        /// <summary>
        /// The vertex normal.
        /// </summary>
        [VertexElement("NORMAL")]
        public Vector3 Normal;

        /// <summary>
        /// Texture.
        /// </summary>
        [VertexElement("TEXCOORD0")]
        public Vector2 TextureCoordinate;

        public bool Equals(VertexPositionNormalTexture other)
        {
            return Position.Equals(other.Position) && Normal.Equals(other.Normal) && TextureCoordinate.Equals(other.TextureCoordinate);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VertexPositionNormalTexture&& Equals((VertexPositionNormalTexture) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Position.GetHashCode();
                hashCode = (hashCode * 397) ^ Normal.GetHashCode();
                hashCode = (hashCode * 397) ^ TextureCoordinate.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(VertexPositionNormalTexture left, VertexPositionNormalTexture right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexPositionNormalTexture left, VertexPositionNormalTexture right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return string.Format("Position: {0}, Normal: {1}, TextureCoordinate: {2}", Position, Normal, TextureCoordinate);
        }
    }
}
