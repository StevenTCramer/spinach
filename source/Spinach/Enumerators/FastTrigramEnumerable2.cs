namespace Spinach.Enumerators;

public class FastTrigramEnumerable2 : IFastEnumerable<IFastEnumerator<TrigramMatchPositionKey, ulong>, TrigramMatchPositionKey, ulong>
{
  // /////////////////////////////////////////////////////////////////////////////////////////////
  // Constructors
  // /////////////////////////////////////////////////////////////////////////////////////////////

  public FastTrigramEnumerable2(string trigram, ITextSearchEnumeratorContext context)
  {
    Trigram = trigram;
    Context = context;
  }

  // /////////////////////////////////////////////////////////////////////////////////////////////
  // Private Properties
  // /////////////////////////////////////////////////////////////////////////////////////////////

  private string Trigram { get; }

  private ITextSearchEnumeratorContext Context { get; }

  // /////////////////////////////////////////////////////////////////////////////////////////////
  // Public Properties
  // /////////////////////////////////////////////////////////////////////////////////////////////

  IEnumerator IEnumerable.GetEnumerator() => GetFastEnumerator();

  public IEnumerator<TrigramMatchPositionKey> GetEnumerator() => GetFastEnumerator();

  public IFastEnumerator<TrigramMatchPositionKey, ulong> GetFastEnumerator()
  {
    // ReSharper disable once ArrangeMethodOrOperatorBody
    return new FastTrigramEnumerator2(Trigram, Context);
  }
}
