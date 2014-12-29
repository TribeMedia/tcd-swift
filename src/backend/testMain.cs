using System;
using System.Collections.Generic;

// Type of an Ident; leaving this as string for now
using Ident = System.String;

/*
 * The different internal types for IR tuples
 * Some of them like LABEL are not really ops
 */
public enum IrOp: int
{
  ADD,
  AND,
  CALL,
  DIV,
  EQU,
  FADD,
  FDIV,
  FMUL,
  FSUB,
  FUNC,
  GT,
  GTE,
  JMP,
  JMPF,
  LABEL,
  LOAD,
  LT,
  LTE,
  MOD,
  MUL,
  NEG,
  NEQ,
  NOT,
  OR,
  RET,
  STORE,
  SUB,
  XOR
};
public class CodeGen{
    public static string IRToARM(IRTuple IR){
        if(IR.getOp() == IrOp.NEG){
            return "Neg " + IR.getDest();
        }
        if(IR.getOp() == IrOp.ADD){
            if(Object.ReferenceEquals(IR.GetType(), typeof(IRTupleTwoOp))){
                return "ADD " + IR.getDest() +", "+ ((IRTupleTwoOp)IR).getSrc1() + ", " + ((IRTupleTwoOp)IR).getSrc2();
            }
        }
        return "";
    }
}
/* Generalized IRTuple class */
public class IRTuple
{
  protected IrOp op;
  protected Ident dest;

  public IRTuple(IrOp irop, Ident destination)
  {
    this.op = irop;
    this.dest = destination;
  }

  public IrOp getOp()
  {
    return this.op;
  }
  
  public Ident getDest()
  {
    return this.dest;
  }

  public virtual void Print()
  {
    Console.Write("{" + Enum.GetName(typeof(IrOp), this.op) + ", " + this.dest);
    Console.WriteLine("}");
  }
}

/* IRTuple with one operand where operand is an Ident */
public class IRTupleOneOpIdent : IRTuple
{
  protected Ident src1;

  public IRTupleOneOpIdent(IrOp irop, Ident destination, Ident source) : base(irop, destination)
  {
    this.src1 = source;
  }

  public Ident getSrc1()
  {
    return this.src1;
  }

  public override void Print()
  {
    Console.Write("{" + Enum.GetName(typeof(IrOp), this.op) + ", " + this.dest);
    Console.Write(", " + this.src1);
    Console.WriteLine("}");
  }
}

/* IRTuple with one operand where operand is an immediate */
public class IRTupleOneOpImm<T> : IRTuple
{
  protected T src1;

  public IRTupleOneOpImm(IrOp irop, Ident destination, T source) : base(irop, destination)
  {
    this.src1 = source;
  }

  public T getSrc1()
  {
    return this.src1;
  }

  public override void Print()
  {
    Console.Write("{" + Enum.GetName(typeof(IrOp), this.op) + ", " + this.dest);
    Console.Write(", " + this.src1);
    Console.WriteLine("}");
  }
}

/* IRTuple with two operands */
public class IRTupleTwoOp : IRTupleOneOpIdent
{
  protected Ident src2;

  public IRTupleTwoOp(IrOp irop, Ident destination, Ident source1, Ident source2) : base(irop, destination, source1)
  {
    this.src2 = source2;
  }

  public Ident getSrc2()
  {
    return this.src2;
  }

  public override void Print()
  {
    Console.Write("{" + Enum.GetName(typeof(IrOp), this.op) + ", " + this.dest);
    Console.Write(", " + this.src1);
    Console.Write(", " + this.src2);
    Console.WriteLine("}");
  }  
}

public class testMain{
    static int Main(string[] args)
    {
        IRTuple testNeg = new IRTuple(IrOp.NEG, "R1");
        testNeg.Print();
        IRTupleTwoOp testAdd = new IRTupleTwoOp(IrOp.ADD, "R1","R1","R2");
        testAdd.Print();
        System.Console.WriteLine("\n"+CodeGen.IRToARM(testNeg));
        System.Console.WriteLine("\n"+CodeGen.IRToARM(testAdd));
        return 0;
    }
}
