//-----------------------------------------------------------------------
// <copyright file="UnmanagedMemoryBlockTest.cs" company="P.O.S Informatique">
// Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.Net.HttpServer.Tests
{
    using System;
    using System.Runtime.InteropServices;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnmanagedMemoryBlockTest
    {
        [TestMethod]
        public void Constructor_With_Default_Length()
        {
            using (var block = new UnmanagedMemoryBlock())
            {
                block.Length.Should().Be(1024);
            }
        }

        [TestMethod]
        public void Constructor_With_Explicit_Length()
        {
            using (var block = new UnmanagedMemoryBlock(2048))
            {
                block.Length.Should().Be(2048);
            }
        }

        [TestMethod]
        public void BlockUsage()
        {
            using (var block = new UnmanagedMemoryBlock(4))
            {
                var pointer = (IntPtr)block;

                // Copy to the unmanaged block
                Marshal.Copy(new byte[] { 1, 2, 3, 4 }, 0, pointer, 4);

                // Retrieve data from unmanaged block
                var bytes = new byte[4];
                Marshal.Copy(pointer, bytes, 0, 4);

                bytes.ShouldBeEquivalentTo(new[] { 1, 2, 3, 4 });
            }
        }

        [TestMethod]
        public void ExtendsUsage()
        {
            using (var block = new UnmanagedMemoryBlock(4))
            {
                var originalPointer = (IntPtr)block;

                block.Extends(6);

                var pointer = (IntPtr)block;
                pointer.Should().NotBe(originalPointer);

                // Copy to the unmanaged block
                Marshal.Copy(new byte[] { 1, 2, 3, 4, 5, 6 }, 0, pointer, 6);

                // Retrieve data from unmanaged block
                var bytes = new byte[6];
                Marshal.Copy(pointer, bytes, 0, 6);

                bytes.ShouldBeEquivalentTo(new[] { 1, 2, 3, 4, 5, 6 });
            }
        }

        [TestMethod]
        public void Unwrap()
        {
            using (var block = new UnmanagedMemoryBlock(4))
            {
                // Copy to the unmanaged block
                Marshal.Copy(new byte[] { 1, 2, 3, 4 }, 0, block, 4);

                // Unwrap the content of the unmanaged block to the StructToUnwrap
                var structure = block.Unwrap<StructureToUnwrap>();
                structure.A.Should().Be(1);
                structure.B.Should().Be(2);
                structure.C.Should().Be(3);
                structure.D.Should().Be(4);
            }
        }

        [TestMethod]
        public void Finalizer()
        {
            var block = new UnmanagedMemoryBlock(4);

            block = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct StructureToUnwrap
        {
            public byte A;
            public byte B;
            public byte C;
            public byte D;
        }
    }
}
