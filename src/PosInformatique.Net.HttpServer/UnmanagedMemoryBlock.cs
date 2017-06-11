//-----------------------------------------------------------------------
// <copyright file="UnmanagedMemoryBlock.cs" company="P.O.S Informatique">
// Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.Net.HttpServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a unmanaged memory block.
    /// </summary>
    internal sealed class UnmanagedMemoryBlock : IDisposable
    {
        /// <summary>
        /// Length of the unmanaged memory block.
        /// </summary>
        private int length;

        /// <summary>
        /// Pointer to the unmanaged memory block.
        /// </summary>
        private IntPtr pointer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnmanagedMemoryBlock"/> class
        /// with a length of 1024 bytes.
        /// </summary>
        public UnmanagedMemoryBlock()
            : this(1024)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnmanagedMemoryBlock"/> class
        /// with the specified <paramref name="initialLength"/> of bytes.
        /// </summary>
        /// <param name="initialLength">Initial length of the memory block.</param>
        public UnmanagedMemoryBlock(int initialLength)
        {
            this.length = initialLength;

            this.pointer = Marshal.AllocHGlobal(initialLength);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="UnmanagedMemoryBlock"/> class.
        /// </summary>
        ~UnmanagedMemoryBlock()
        {
            this.Dispose();
        }

        /// <summary>
        /// Gets the length of the unmanaged block.
        /// </summary>
        public int Length
        {
            get
            {
                return this.length;
            }
        }

        /// <summary>
        /// Gets the pointer to the specified unmanaged <paramref name="memory"/> block.
        /// </summary>
        /// <param name="memory"><see cref="UnmanagedMemoryBlock"/> instance which the pointer have to be retrieved from.</param>
        public static implicit operator IntPtr(UnmanagedMemoryBlock memory)
        {
            return memory.pointer;
        }

        /// <summary>
        /// Extends the current <see cref="UnmanagedMemoryBlock"/> with the specified <paramref name="length"/>.
        /// </summary>
        /// <param name="length">New length of the current <see cref="UnmanagedMemoryBlock"/>.</param>
        public void Extends(int length)
        {
            Marshal.FreeHGlobal(this.pointer);

            this.length = length;
            this.pointer = Marshal.AllocHGlobal(this.length);
        }

        /// <summary>
        /// Unwraps the current <see cref="UnmanagedMemoryBlock"/> content to an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Structure type which the content of the current <see cref="UnmanagedMemoryBlock"/> have to be unwrapped.</typeparam>
        /// <returns>A new instance of <typeparamref name="T"/> of the current <see cref="UnmanagedMemoryBlock"/> unwrapped.</returns>
        public T Unwrap<T>()
            where T : struct
        {
            return (T)Marshal.PtrToStructure(this.pointer, typeof(T));
        }

        /// <summary>
        /// Releases all managed and non managed resources.
        /// </summary>
        public void Dispose()
        {
            if (this.pointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.pointer);
                this.pointer = IntPtr.Zero;
            }
        }
    }
}
