using Decodex.Cards;
using Grim.Items;
using NSubstitute;
using NUnit.Framework;

public class CardInstanceTests
{
    private ICard GetCard()
        => new Card(
            "PRT_000",
            new(new[] { PrimeAttribute.Aegis }),
            new(new[] { PrimeAttribute.Ruin })
        );

    [Test]
    public void New()
    {
        var card = GetCard();
        var cardInstance = new CardInstance("1", card);
        
        Assert.AreEqual("1", cardInstance.Id);
        Assert.AreEqual("PRT_000", cardInstance.SetId);
        Assert.AreEqual(1, cardInstance.MemoryCells.Count);
        Assert.Contains(PrimeAttribute.Aegis, cardInstance.MemoryCells);
        Assert.AreEqual(1, cardInstance.MemoryRequirement.Count);
        Assert.Contains(PrimeAttribute.Ruin, cardInstance.MemoryRequirement);
    }

    [Test]
    public void ModifierOne()
    {
        var card = GetCard();
        var cardInstance = new CardInstance("1", card);
        var modifier = Substitute.For<IModifier<ICard>>();
        modifier.Layer.Returns("LAYER_1");
        modifier.Apply(Arg.Any<ICard>()).Returns(new Card("PRT_001", new(), new()));

        cardInstance.AddModifier(modifier);

        Assert.AreEqual("1", cardInstance.Id);
        Assert.AreEqual("PRT_001", cardInstance.SetId);
        Assert.AreEqual(0, cardInstance.MemoryCells.Count);
        Assert.AreEqual(0, cardInstance.MemoryRequirement.Count);
    }
}
