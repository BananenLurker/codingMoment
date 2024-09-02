namespace Leet
{
    internal class Program
    {
        public class ListNode
        {
            public int val;
            public ListNode next;
            public ListNode(int val = 0, ListNode next = null)
            {
                this.val = val;
                this.next = next;
            }
        }

        static void Main()
        {
            int n = 1;
            ListNode tail = new ListNode(2);
            ListNode head = new ListNode(1, tail);
            ListNode copy = head;
            int count = 0;
            while (head != null)
            {
                head = head.next;
                count++;
            }
            int target = count - n;
            Console.WriteLine(target);
            Console.WriteLine("copy val: " + copy.val);
            head = copy;
            Console.WriteLine("head.val: " + head.val);
            for (int i = 0; i < target; i++)
            {
                Console.WriteLine("current i: " + i);
                head = head.next;
            }
            if (head.next.next != null)
            {
                Console.WriteLine(head.next.next.val);
                head.next = head.next.next;
            }
            else
            {
                Console.WriteLine("oeps");
                head.next = null;
            }
        }
    }
}