// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("4iA4yDon8otjUZskkZFwg1oeV8rTRYQEePgL5ypWly0NeE2CCJmx92rp5+jYauni6mrp6ehEtex0LTH+0G91LHq2PUKIdo2Dlh6OpAduLHsFtcT+uTv5ZbSdND1MClQdvby4SjqFI3PKDWZg1CominXXTKI3bpmTJvpEv3rXOu0uxwkJlL05w9FfZGOYpJIfzEyqkADGcgq67NkB3Abeios1HK4HNwTovmkiuW4bT7l6Vto+RBVVLwtq6GyZtz4A1hqV3AZFV6caxd2/l7UIzpjlHCaZ09q7zAz1dbhQ41o0W4FUbOEQ+L0oYiUkznxsxAj3MIpSOSthqYNJOJ2tBV0JuuzYaunK2OXu4cJuoG4f5enp6e3o637+vb0W+lYh/+rr6ejp");
        private static int[] order = new int[] { 11,7,7,9,12,7,12,10,9,13,13,12,13,13,14 };
        private static int key = 232;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
