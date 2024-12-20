namespace Spinach.Enumerators;

public class FastTrigramEnumerator : IFastEnumerator<MatchWithRepoOffsetKey, ulong>
{
  // /////////////////////////////////////////////////////////////////////////////////////////////
  // Constructors
  // /////////////////////////////////////////////////////////////////////////////////////////////

  public FastTrigramEnumerator(
    DiskBTree<int, long> trigramTree,
    LruCache<TrigramMatchCacheKey, DiskSortedVarIntList> postingsListCache,
    DiskSortedVarIntListFactory sortedVarIntListFactory,
    DiskBTreeFactory<TrigramMatchKey, long> trigramMatchesFactory,
    int trigramKey
  )
  {
    TrigramTree = trigramTree;
    PostingsListCache = postingsListCache;
    SortedVarIntListFactory = sortedVarIntListFactory;
    TrigramMatchesFactory = trigramMatchesFactory;
    TrigramKey = trigramKey;
    CurrentKey = new MatchWithRepoOffsetKey();

    Reset();
  }

  // /////////////////////////////////////////////////////////////////////////////////////////////
  // Private Properties
  // /////////////////////////////////////////////////////////////////////////////////////////////

  private DiskSortedVarIntListFactory SortedVarIntListFactory { get; set; }

  private DiskBTreeFactory<TrigramMatchKey, long> TrigramMatchesFactory { get; set; }

  private LruCache<TrigramMatchCacheKey, DiskSortedVarIntList> PostingsListCache { get; set; }

  private DiskSortedVarIntList PostingsList { get; set; }

  private DiskBTreeCursor<TrigramMatchKey, long> TrigramMatchesCursor { get; set; }

  private DiskSortedVarIntListCursor PostingsListCursor { get; set; }

  private int TrigramKey { get; }

  private DiskBTree<int, long> TrigramTree { get; set; }

  // /////////////////////////////////////////////////////////////////////////////////////////////
  // Public Properties
  // /////////////////////////////////////////////////////////////////////////////////////////////

  object IEnumerator.Current => Current;

  public ulong Current => CurrentData;

  public ulong CurrentData => (ulong)CurrentKey.Offset;

  public MatchWithRepoOffsetKey CurrentKey { get; private set; }

  // /////////////////////////////////////////////////////////////////////////////////////////////
  // Public Methods
  // /////////////////////////////////////////////////////////////////////////////////////////////

  public void Dispose()
  {
  }

  public bool MoveNext()
  {
    if (PostingsListCursor.MoveNext())
    {
      CurrentKey.Offset = PostingsListCursor.CurrentData;
      return true;
    }

    if (!TrigramMatchesCursor.MoveNext()) return false;

    CurrentKey.UserType = TrigramMatchesCursor.CurrentKey.UserType;
    CurrentKey.UserId = TrigramMatchesCursor.CurrentKey.UserId;
    CurrentKey.RepoId = TrigramMatchesCursor.CurrentKey.RepoId;

    PostingsList = SortedVarIntListFactory.LoadExisting(TrigramMatchesCursor.CurrentData);
    PostingsListCursor = new DiskSortedVarIntListCursor(PostingsList);
    PostingsListCursor.Reset();

    bool result = PostingsListCursor.MoveNext();
    CurrentKey.Offset = (int)PostingsListCursor.CurrentKey;

    return result;
  }

  public bool MoveUntilGreaterThanOrEqual(MatchWithRepoOffsetKey target)
  {
    if (CurrentKey.UserType == target.UserType &&
        CurrentKey.UserId == target.UserId &&
        CurrentKey.RepoId == target.RepoId)
    {
      if (PostingsListCursor.MoveUntilGreaterThanOrEqual((ulong)target.Offset))
      {
        CurrentKey.Offset = (long)PostingsListCursor.CurrentKey;
        return true;
      }
    }

    TrigramMatchesCursor.MoveNext();
    var nextMatchKey = new TrigramMatchKey(target.UserType, target.UserId, target.RepoType, target.RepoId);
    bool hasNextMatch = TrigramMatchesCursor.MoveUntilGreaterThanOrEqual(nextMatchKey);

    if (!hasNextMatch) return false;

    PostingsList = SortedVarIntListFactory.LoadExisting(TrigramMatchesCursor.CurrentData);
    PostingsListCursor = new DiskSortedVarIntListCursor(PostingsList);
    PostingsListCursor.Reset();
    PostingsListCursor.MoveNext();

    return true;
  }

  public void Reset()
  {
    // if (!TrigramPostingsListCache.TryGetValue(TrigramKey, out DiskSortedVarIntList postingsList))
    // {
    //   if (TrigramTree.TryFind(TrigramKey, out long postingsListAddress))
    //   {
    //     postingsList = SortedVarIntListFactory.LoadExisting(postingsListAddress);
    //     TrigramPostingsListCache.Add(TrigramKey, postingsList);
    //   }
    // }
    //
    // PostingsList = postingsList;
    // PostingsListCursor = new DiskSortedVarIntListCursor(PostingsList);
    // PostingsListCursor.Reset();
  }
}
