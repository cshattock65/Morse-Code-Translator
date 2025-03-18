using System.IO.Compression;

public static class CompressDecompress
{
    // Compresses the input byte array using GZip compression.
    public static byte[] Compress(byte[] buffer)
    {
        // Creating a memory stream to hold the compressed data.
        using var memStream = new MemoryStream();

        // Creating a compression stream that writes to the memory stream.
        using (var gZipStream = new GZipStream(memStream, CompressionMode.Compress, true))
        {
            // Writing the data to be compressed into the compression stream.
            gZipStream.Write(buffer, 0, buffer.Length);
        }
        // Converting the memory stream to an array and return the compressed data.
        return memStream.ToArray();
    }

    // Decompresses the input byte array using GZip decompression.
    public static byte[] Decompress(byte[] compressedData)
    {
        // Creating a memory stream with the compressed data.
        using var memStream = new MemoryStream(compressedData);

        // Creating a decompression stream that reads from the memory stream.
        using var gZipStream = new GZipStream(memStream, CompressionMode.Decompress);

        // Creating a memory stream to collect the decompressed data.
        using var resultStream = new MemoryStream();

        // Copying data from the decompression stream into the result stream.
        gZipStream.CopyTo(resultStream);

        // Converting the result stream to an array and return the decompressed data.
        return resultStream.ToArray();
    }
}
