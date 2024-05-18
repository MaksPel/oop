using System;

public interface IDecoratedStream
{
    void Write(byte[] buffer, int offset, int count);
    string GetLastFiveCharacters();
}
