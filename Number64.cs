namespace Program
{
    public class Number64
    {
        private uint left_;
        private uint right_;

        public Number64()
        {

        }

        public Number64(uint left, uint right)
        {
            this.left_ = left;
            this.right_ = right;
        }

        public uint left
        {
            get { return left_; }
            set { left_ = value; }
        }

        public uint right
        {
            get { return right_; }
            set { right_ = value; }
        }

        public Number64 Recreate(ulong number)
        {
            this.left = (uint)(number >> 32);
            this.right = (uint)number;

            return this;
        }

        public override string ToString()
        {
            return this.left + "" + this.right;
        }

        public static implicit operator Number64(ulong x)
        {
            uint left = (uint)(x >> 32);
            uint right = (uint)x;

            return new Number64(left, right);
        }

        public static explicit operator ulong(Number64 x)
        {
            return (ulong)(x.left_) << 32 | x.right_;
        }
    }
}
