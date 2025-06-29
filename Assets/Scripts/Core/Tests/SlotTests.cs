using System;
using TheChest.Examples.Items;
using NUnit.Framework;
using TheChest.Examples.Containers;

namespace TheWorld.Tests.TheChest
{
    public partial class SlotTests
    {
        public Random random;

        const int low_amount = 10;
        const int high_amount = 20;

        public SlotTests()
        {
            random = new Random();

            Assert.IsTrue(low_amount < high_amount);
        }

        private Item DefaultItemGenerator()
        {
            return new Item(
              id: Guid.NewGuid().ToString(),
              name: Guid.NewGuid().ToString(),
              description: Guid.NewGuid().ToString(),
              image: null,
              maxStack: random.Next(1, high_amount)
           );
        }

        [Test]
        public void SlotConstrutorEmpty()
        {
            var slot = new StackSlot();
            Assert.IsTrue(slot.IsEmpty);
            Assert.IsNull(slot.CurrentItem);
        }

        [Test]
        public void SlotConstrutorWithItem()
        {
            var item = this.DefaultItemGenerator();
            var slot = new StackSlot(item);

            Assert.IsFalse(slot.IsEmpty);
            Assert.IsNotNull(slot.CurrentItem);
            Assert.AreEqual(item, slot.CurrentItem);
        }
    }
}
