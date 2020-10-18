using System;

namespace RaphsTools
{
    public class PieSection
    {
        // Pointer to the current lower bound
        protected float _lowerBound;

        // Pointer to the current upper bound
        protected float _upperBound;

        // Indicator of current pie section size
        protected readonly float _sectionSize;

        // Indicator of current pie section absolute center. Equals to _sectionSize / 2 all the time.
        protected readonly float _absoluteCenter;

        // Indicator of current full pie size
        protected readonly float _pieSize;

        public PieSection(float sectionSize, float pieSize)
        {
            if (sectionSize >= pieSize)
            {
                throw new Exception("Section size must be smaller than the pie size");
            }
            _sectionSize = sectionSize;
            _absoluteCenter = _sectionSize / 2;
            _pieSize = pieSize;
            _lowerBound = 0;
            _upperBound = sectionSize;
        }

        // Note that if you need other functionnality such as diff with lower bound or diff with upper bound, it can be done easily based on this one.
        // Just need to call fitBoundsToValue(inputValue) and then unSkew the inputValue and do your calculations.
        // If you want to submit a PR with your implementations and unit tests, I will welcome those with open arms
        public float diffWithCenter(float inputValue)
        {
            fitBoundsToValue(inputValue);

            // Whether we adjusted the bounds or not, we now need to find the distance of the point to the center of the pie section
            // We do that by essentially getting rid of the offsets that we are currently in, so setting all our values back to [0, _sectionSize].
            // When shifting is done, we can get _sectionSize / 2 to get the center of the section, and then do a difference with our inputValue
            // that has been shifted back to a [0, _sectionSize] range.
            return -(_absoluteCenter - unSkew(inputValue));
        }

        private void fitBoundsToValue(float inputValue)
        {
            if (valueInBounds(inputValue))
            {
                return;
            }

            // Compute the direct (or trivial) distances, going from the inputValue to the bound

            // DLB means Distance to lower bound. 
            float DLB = Math.Abs(inputValue - _lowerBound);
            // DUB means Distance to upper bound
            float DUB = Math.Abs(inputValue - _upperBound);

            // In this case, the _lowerBound is larger than the _upperBound, because the section is crossing the pie's crossover point
            // Example: for a 360 degrees circle, the section could be at [315, 45] for a 90 degree Pie Section.
            if (inputValue < _lowerBound && inputValue > _upperBound)
            {
                // We can directly pick the closest bound and pin it to the value that has been passed
                if (DUB < DLB)
                {
                    upperBound = inputValue;
                }
                else
                {
                    lowerBound = inputValue;
                }
            }
            // Now we find that the value input is lower in value than both bounds.
            // The instinct is to pick the lower bound as it seems closest, but we actually need to go all the way around
            //  the pie the other way to make sure the upper bound isn't closer this way
            else if (inputValue < _lowerBound && inputValue < _upperBound)
            {
                // Round trip around earth guys. It's happening
                // DUBPrime is the distance to get to UpperBound going the other way around (over the 0)
                float DUBPrime = _pieSize - _upperBound + inputValue;
                if (DLB < DUB && DLB < DUBPrime)
                {
                    lowerBound = inputValue;
                }
                else
                {
                    upperBound = inputValue;
                }
            }
            // Now we find that the value input is higher in value than both bounds.
            // The instinct is to pick the upper bound as it seems closest, but we actually need to go all the way around
            //  the pie the other way to make sure the lower bound isn't closer this way
            else
            {
                // Round trip around earth guys. It's happening
                // DLBPrime is the distance to get to LowerBound going the other way around (over the 0)
                float DLBPrime = _pieSize - inputValue + _lowerBound;
                if (DUB < DLB && DUB < DLBPrime)
                {
                    upperBound = inputValue;
                }
                else
                {
                    lowerBound = inputValue;
                }
            }
        }

        // Normalizes the value to a simple [0, _sectionSize] model, by shifting it back as much as the _lowerBounds is shifted
        // Note that depending on the value passed in, the output could be larger than _sectionSize if the value is outside of the bounds to begin with.
        // Visually, this is what happens:
        // Where l is the lower bound, * is the value, u is the upper bound and | is the pieSize
        // Skewed value:    0-------l---*-u-----|
        // UnSkewed value:  l---*-u-------------|
        // Also note that this example supposes the bounds are moving, but this function only unskewes the * (value) and does not touch the actual bounds.
        // Unskewed bounds are easy to guess as they are 0 and _sectionSize respectively when value is unskewed.
        private float unSkew(float value)
        {
            return wrap(value - _lowerBound);
        }

        // We get rid of the offsets by going back to a simple [0, _sectionSize] model
        // And then use that to make sure the value is actually in the [0, _sectionSize] range
        private bool valueInBounds(float val)
        {
            float unSkewedVal = unSkew(val);
            return unSkewedVal >= 0 && unSkewedVal <= _sectionSize;
        }

        // When we get the center, since it's not stored anywhere, we calculate it by either removing half the width of the pie section of the _upperBound
        // or adding it to the _lowerBound
        public float center
        {
            get { return wrap(_lowerBound + _absoluteCenter); }
            set {
                // We can also set the bounds by hard-passing a center value, which will set both upper and lower bounds by half the pie section size
                _lowerBound = wrap(value - _absoluteCenter);
                _upperBound = wrap(value + _absoluteCenter);
            }
        }

        public float lowerBound
        {
            get { return _lowerBound; }
            set {
                // We only need to calculate the upper bound by offsetting the lowerbound by the section size
                _lowerBound = wrap(value);
                _upperBound = wrap(_lowerBound + _sectionSize);
            }
        }

        public float upperBound
        {
            get { return _upperBound; }
            set {
                // We only need to calculate the lower bound by offsetting the upperbound by the section size
                _upperBound = wrap(value);
                _lowerBound = wrap(_upperBound - _sectionSize);
            }
        }

        public float pieSize
        {
            get { return _pieSize; }
        }

        public float sectionSize
        {
            get { return _sectionSize; }
        }

        // Math implementation of Unity's "Mathf.Repeat". Is used to constraint values between 0 and _pieSize,
        // but will also repeat if the value is lower or higher than the limits
        private float wrap(float value)
        {
            float wrappedValue = value % _pieSize;
            if (wrappedValue < 0)
            {
                wrappedValue += _pieSize;
            }
            return wrappedValue;
        }
    }
}
