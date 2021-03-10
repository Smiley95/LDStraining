using System;
using Xunit;
using Probabilities;

namespace ProbabilityTests
{
    public class ProbabilityTest
    {

        [Trait("Category", "Calculation")]
        [Fact]
        public void can_calculate_probability_of_both_events()
        {
            var probability = new Probability();
            Assert.Equal(0.3m, probability.CalculateProbabilityOfBoth(0.6m, 0.5m));
        }

        [Trait("Category", "Calculation")]
        [Fact]
        public void can_calculate_probability_of_either_events()
        {
            var probability = new Probability();
            Assert.Equal(0.8m, probability.CalculateProbabilityOfEither(0.6m, 0.5m));
        }

        [Trait("Category", "Calculation")]
        [Fact]
        public void can_calculate_probability_of_inverse_event()
        {
            var probability = new Probability();
            Assert.Equal(0.4m, probability.CalculateProbabilityOfInverse(0.6m));
        }

        [Trait("Category", "Calculation")]
        [Fact]
        public void cannot_use_wrong_params()
        {
            var probability = new Probability();
            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfBoth(-0.1m, 0.2m));
            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfBoth(0.1m, -0.2m));
            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfBoth(0.1m, 1.2m));
            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfBoth(1.1m, 0.2m));

            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfEither(-0.1m, 0.2m));
            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfEither(0.1m, -0.2m));
            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfEither(0.1m, 1.2m));
            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfEither(1.1m, 0.2m));

            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfInverse(-0.1m));
            Assert.Throws<ArgumentException>(() => probability.CalculateProbabilityOfInverse(1.1m));

        }
    }
}
