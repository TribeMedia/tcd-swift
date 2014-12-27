using System;
using System.Collections.Generic;

using Ident = System.String;

/* A basic block of IR */
public class IRBlock
{
  private int index; // The index number of this block within the graph
  private List<IRTuple> statements;
  private List<IRBlock> successors;
  private HashSet<Ident> liveuse;
  private HashSet<Ident> def;
  private HashSet<Ident> livein;
  private HashSet<Ident> liveout;

  public IRBlock(int ind)
  {
    this.index = ind;
    this.statements = new List<IRTuple>();
    this.successors = new List<IRBlock>();

    this.liveuse = new HashSet<Ident>();
    this.def = new HashSet<Ident>();
    this.livein = new HashSet<Ident>();
    this.liveout = new HashSet<Ident>();
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

  // Return a set of all identifiers used or defined in this block
  public HashSet<Ident> GetVarNames()
  {
    HashSet<Ident> vars = new HashSet<Ident>();
    foreach (IRTuple irt in this.statements)
    {
      vars.UnionWith(irt.GetUsedVars());
      vars.UnionWith(irt.GetDefinedVars());
    }
    return vars;
  }

  // Compute event(LiveUse) and anti-event(Def) sets for this block
  public void ComputeLiveuseDef()
  {
    HashSet<Ident> used = new HashSet<Ident>();
    HashSet<Ident> defined = new HashSet<Ident>();

    foreach (IRTuple irt in this.statements)
    {
      HashSet<Ident> usedvars = irt.GetUsedVars();
      foreach (Ident ident in usedvars)
      {
        if(!defined.Contains(ident))
          this.liveuse.Add(ident); // Add to liveuse any variables used before they are defined
        used.Add(ident);
      }

      HashSet<Ident> definedvars = irt.GetDefinedVars();
      foreach (Ident ident in definedvars)
      {
        if(!used.Contains(ident))
          this.def.Add(ident); // Add to def any variables defined before they are used
        defined.Add(ident);
      }
    }
  }

  // Update the LiveIn and LiveOut sets
  public void UpdateLiveness()
  {
    foreach (IRBlock irb in this.successors)
      this.liveout.UnionWith(irb.livein); // LiveOut_i <- Union_(j in succ(i)) LiveIn_j

    this.livein.UnionWith(this.liveuse); // LiveIn_i <- LiveUse_i + ...
    foreach (Ident ident in this.liveout){
      if(!this.def.Contains(ident)) // ... LiveOut_i . not(Def_i)
        this.livein.Add(ident);
    }
  }

  public HashSet<Ident> GetLiveIn()
  {
    return this.livein;
  }

  public HashSet<Ident> GetLiveOut()
  {
    return this.liveout;
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

  public void PrintLiveuseDef()
  {
    Console.Write("Variables: ");
    HashSet<Ident> vars = this.GetVarNames();
    foreach (Ident ident in vars)
      Console.Write(ident + " ");
    Console.WriteLine();

    Console.Write("LiveUse: ");
    foreach (Ident ident in liveuse)
      Console.Write(ident + " ");
    Console.WriteLine();

    Console.Write("Def: ");
    foreach (Ident ident in def)
      Console.Write(ident + " ");
    Console.WriteLine();
  }

  public void PrintLiveInOut()
  {
    Console.Write("LiveIn: ");
    foreach (Ident ident in this.livein)
      Console.Write(ident + " ");
    Console.WriteLine();

    Console.Write("LiveOut: ");
    foreach (Ident ident in this.liveout)
      Console.Write(ident + " ");
    Console.WriteLine();
  }
}

