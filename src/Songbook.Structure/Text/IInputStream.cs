namespace Songbook.Text
{
    public interface IInputStream<T>
    {
        bool IsMarked { get; }

        int Position { get; }

        T[] ClearMark();

        T Consume();

        T Consume(int count);

        T LA(int count);

        void Mark();
    }
}
