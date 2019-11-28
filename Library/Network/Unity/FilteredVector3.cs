using UnityEngine;

namespace Pyro.Network.Unity
{
    public class FilteredVector3
    {
        private readonly FilterButterworth _x, _y, _z;

        public FilteredVector3(float frequency, int sampleRate, float resonance = 1.414213562373095f)
        {
            _x = new FilterButterworth(frequency, sampleRate, FilterButterworth.PassType.Lowpass, resonance); 
            _y = new FilterButterworth(frequency, sampleRate, FilterButterworth.PassType.Lowpass, resonance);
            _z = new FilterButterworth(frequency, sampleRate, FilterButterworth.PassType.Lowpass, resonance);
        }

        public void Update(Vector3 v)
        {
            _x.Update(v.x);
            _y.Update(v.y);
            _z.Update(v.z);
        }

        public Vector3 Value
            => new Vector3(_x.Value, _y.Value, _z.Value);
    }
}
