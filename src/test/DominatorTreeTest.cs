using System;
using System.Collections.Generic;
using TCDSwift;

public class DominatorTreeTest
{

  public static void Main(string [] args)
  {  
    TestFindReachableBlocks1();
    TestFindReachableBlocks3();
    TestFindReachableBlocks4();

    Console.WriteLine("*** SUCCESS: Dominator Tree");
  }

  public static void TestFindReachableBlocks1()
  {
    IRGraph cfg = BuildSampleCFG();
    SortedSet<IRBlock> result = DominatorTree.FindReachableBlocks(cfg, 1);
    SortedSet<int> intResult = ConvertToIndexSet(result);

    SortedSet<int> expected = SortedSet<int>();
    expected.Add(1);

    Assert.AreEqual(intResult, expected);
  }

  public static void TestFindReachableBlocks3()
  {
    IRGraph cfg = BuildSampleCFG();
    SortedSet<IRBlock> result = DominatorTree.FindReachableBlocks(cfg, 3);
    SortedSet<int> intResult = ConvertToIndexSet(result);

    SortedSet<int> expected = SortedSet<int>();
    expected.Add(1);
    expected.Add(2);

    Assert.AreEqual(intResult, expected);
  }

  public static void TestFindReachableBlocks4()
  {
    IRGraph cfg = BuildSampleCFG();
    SortedSet<IRBlock> result = DominatorTree.FindReachableBlocks(cfg, 4);
    SortedSet<int> intResult = ConvertToIndexSet(result);

    SortedSet<int> expected = SortedSet<int>();
    expected.Add(1);
    expected.Add(2);
    expected.Add(3);
    expected.Add(5);
    expected.Add(8);

    Assert.AreEqual(intResult, expected);
  }

  public static void TestBuildFullTree()
  {
    // Build Dominator Tree
    cfg = BuildSampleCFG();
    DominatorTree dominatorTree = DominatorTree(cfg);

    // Compare Result to Expected
  }

  private static IRGraph BuildSampleCFG()
  {
    IRBlock block1 = new IRBlock(1);
    IRBlock block2 = new IRBlock(2);
    IRBlock block3 = new IRBlock(3);
    IRBlock block4 = new IRBlock(4);
    IRBlock block5 = new IRBlock(5);
    IRBlock block6 = new IRBlock(6);
    IRBlock block7 = new IRBlock(7);
    IRBlock block8 = new IRBlock(8);
    IRBlock block9 = new IRBlock(9);

    block1.AddSuccessor(block2);
    block2.AddSuccessor(block3);
    block3.AddSuccessor(block4);
    block3.AddSuccessor(block5);
    block4.AddSuccessor(block6);
    block4.AddSuccessor(block7);
    block5.AddSuccessor(block8);
    block6.AddSuccessor(block9);
    block7.AddSuccessor(block9);
    block9.AddSuccessor(block4);

    List<IRBlock> blocks = List<IRBlock>();
    blocks.Add(block1);
    blocks.Add(block2);
    blocks.Add(block3);
    blocks.Add(block4);
    blocks.Add(block5);
    blocks.Add(block6);
    blocks.Add(block7);
    blocks.Add(block8);
    blocks.Add(block9);

    IRGraph cfg = IRGraph(blocks);
  }

  private static SortedSet<int> ConvertToIndexSet(SortedSet<IRBlock> blocks)
  {
    SortedSet<int> intSet = SortedSet<int>();

    foreach (IRBlock block in blocks)
    {
      intSet.Add(block.GetIndex());
    }

    return intSet;
  }

}