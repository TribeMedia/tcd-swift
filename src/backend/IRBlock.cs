using System;
using System.Collections.Generic;

/* A basic block of IR */
public class IRBlock
{
  private int index; // The index number of this block within the graph
  private List<IRTuple> statements;
  private List<IRBlock> successors;

  public IRBlock(int ind)
  {
    this.index = ind;
    this.statements = new List<IRTuple>();
    this.successors = new List<IRBlock>();
  }

  public int GetIndex()
  {
    return this.index;
  }

  /* Forwarding functions */
  public void AppendStatement(IRTuple stat)
  {
    this.statements.Add(stat);
  }

  public void InsertStatement(IRTuple stat, int index)
  {
    this.statements.Insert(index, stat);
  }

  public void RemoveStatement(IRTuple stat)
  {
    this.statements.Remove(stat);
  }

  public void RemoveStatementAt(int index)
  {
    this.statements.RemoveAt(index);
  }

  public IRTuple GetStatement(int index)
  {
    return this.statements[index];
  }

  public IRTuple GetFirst()
  {
    return this.statements[0];
  }

  public IRTuple GetLast()
  {
    return this.statements[this.statements.Count-1];
  }

  public int CountStatements()
  {
    return this.statements.Count;
  }

  public void AddSuccessor(IRBlock block)
  {
    this.successors.Add(block);
  }

  public void RemoveSuccessor(IRBlock block)
  {
    this.successors.Remove(block);
  }

  public void RemoveSuccessorAt(int index)
  {
    this.successors.RemoveAt(index);
  }

  public IRBlock GetSuccessor(int index)
  {
    return this.successors[index];
  }

  public void PrintStatements()
  {
    foreach (IRTuple irt in this.statements)
    {
      Console.Write("{" + Enum.GetName(typeof(IrOp), irt.getOp()) + ", " + irt.getDest());

      if(irt is IRTupleOneOpIdent)
        Console.Write(", " + ((IRTupleOneOpIdent)irt).getSrc1());
      else if(irt is IRTupleOneOpImm<int>)
        Console.Write(", " + ((IRTupleOneOpImm<int>)irt).getSrc1());

      if(irt is IRTupleTwoOp)
        Console.Write(", " + ((IRTupleTwoOp)irt).getSrc2());

      Console.WriteLine("}");
    }
  }

  public void PrintSuccessors()
  {
    Console.Write("Successors: ");
    foreach (IRBlock irb in this.successors)
    {
      Console.Write("B" + irb.GetIndex() + " ");
    }
    Console.WriteLine();
  }
}

