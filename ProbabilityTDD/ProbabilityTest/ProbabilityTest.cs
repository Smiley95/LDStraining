using System;
using Xunit;
using Probability;

namespace ProbabilityTest
{
    public class ProbabilityTest
    {
        [Trait("Category","Initiation")]
        [Fact]
        public void can_initiate_probability_without_params()
        {
            Assert.NotNull(new Probability());

        }

        [Trait("Category", "Calculation")]
        [Fact]
        public void can_calculate_probability_of_both_events()
        {
            var probability = new Probability();
            Assert.Equal(0.3, probability.CalculateProbabilityOfBoth(0.6,0.5));
        }

        [Trait("Category", "Calculation")]
        [Fact]
        public void can_calculate_probability_of_either_events()
        {
            var probability = new Probability();
            Assert.Equal(0.8, probability.CalculateProbabilityOfEither(0.6, 0.5));
        }

        [Trait("Category", "Calculation")]
        [Fact]
        public void can_calculate_probability_of_inverse_event()
        {
            var probability = new Probability();
            Assert.Equal(0.4, probability.CalculateProbabilityOfInverse(0.6));
        }

        [Trait("Category", "Calculation")]
        [Fact]
        public void cannot_use_wrong_params()
        {
            var probability = new Probability();
            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfBoth(-0.1, 0.2));
            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfBoth(0.1, -0.2));
            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfBoth(0.1, 1.2));
            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfBoth(1.1, 0.2));

            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfEither(-0.1, 0.2));
            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfEither(0.1, -0.2));
            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfEither(0.1, 1.2));
            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfEither(1.1, 0.2));

            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfInverse(-0.1));
            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfInverse(1.1));

        }
    }
}
