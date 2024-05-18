using System;
using System.IO;
using System.Text;

public class LastFiveCharactersStreamDecorator : Stream, IDecoratedStream
{
    private readonly Stream _stream;

    public LastFiveCharactersStreamDecorator(Stream stream)
    {
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _stream.Write(buffer, offset, count);
    }

    public override bool CanRead => _stream.CanRead;
    public override bool CanSeek => _stream.CanSeek;
    public override bool CanWrite => _stream.CanWrite;
    public override long Length => _stream.Length;
    public override long Position
    {
        get => _stream.Position;
        set => _stream.Position = value;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return _stream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _stream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        _stream.SetLength(value);
    }

    public string GetLastFiveCharacters()
    {
        long currentPosition = _stream.Position;
        int length = (int)Math.Min(5, _stream.Length);
        byte[] buffer = new byte[length];
        _stream.Seek(-length, SeekOrigin.End);
        _stream.Read(buffer, 0, length);
        _stream.Seek(currentPosition, SeekOrigin.Begin);
        return Encoding.UTF8.GetString(buffer);
    }

    public override void Flush()
    {
        throw new NotImplementedException();
    }
}
