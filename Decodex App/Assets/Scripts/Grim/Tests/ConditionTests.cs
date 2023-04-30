using Grim.Rules;
using NUnit.Framework;
using System;

public class ConditionTests
{
    private class TestArguments
    {
        public int anInteger;
        public string aString;
        public float aFloat;
        public TestArguments anObject;
    }

    private TestArguments GenerateTestArguments()
    {
        TestArguments result = new();
        result.anInteger = 3;
        result.aString = "hello";
        result.aFloat = 1.4f;
        result.anObject = new();
        result.anObject.anInteger = 2;
        result.anObject.aString = "bbye";
        result.anObject.aFloat = 4.1f;
        return result;
    }

    private Condition<TestArguments> TrueCondition() => new("true", x => true);
    private Condition<TestArguments> FalseCondition() => new("false", x => false);

    [Test]
    public void True()
    {
        TestArguments args = GenerateTestArguments();
        Condition<TestArguments> atom = TrueCondition();
        Assert.IsTrue(atom.Evaluate(args));
    }

    [Test]
    public void False()
    {
        TestArguments args = GenerateTestArguments();
        Condition<TestArguments> atom = FalseCondition();
        Assert.IsFalse(atom.Evaluate(args));
    }

    [Test]
    public void Evaluation()
    {
        TestArguments args = GenerateTestArguments();
        Condition<TestArguments> atom1 = new("id", x => x.anInteger > 2);
        Condition<TestArguments> atom2 = new("id", x => x.anInteger < 2);
        Assert.IsTrue(atom1.Evaluate(args));
        Assert.IsFalse(atom2.Evaluate(args));
    }

    [Test]
    public void New()
    {
        Assert.Throws<ArgumentNullException>(() => new Condition<TestArguments>(null, null));
        Assert.Throws<ArgumentNullException>(() => new Condition<TestArguments>("id", null));
        Assert.Throws<ArgumentNullException>(() => new Condition<TestArguments>(null, x => true));
        Assert.DoesNotThrow(() => new Condition<TestArguments>("id", x => true));
    }
}