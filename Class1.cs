using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp3
{
    class Node<T>
    {
        private T info;
        private Node<T> next;
        /* הפעולה בונה ומחזירה חוליה שהערך שלה הוא info ואין לה חוליה עוקבת **/
        public Node(T info)
        {
            this.info = info;
            this.next = null;
        }
        /*הפעולה בונה ומחזירה חוליה, שהערך שלה הוא info
          והחוליה העוקבת לה היא החוליה next */
        public Node(T info, Node<T> next)
        {
            this.info = info;
            this.next = next;
        }
        /* הפעולה מחזירה את הערך של החוליה הנוכחית **/
        public T GetInfo()
        {
            return info;
        }
        /* הפעולה מחזירה את החוליה העוקבת לחוליה הנוכחית **/
        public Node<T> GetNext()
        {
            return next;
        }
        /* הפעולה קובעת את ערך החוליה הנוכחית להיות  info **/
        public void SetInfo(T info)
        {
            this.info = info;
        }
        /* הפעולה קובעת את החוליה העוקבת לחוליה הנוכחית להיות החוליה next **/
        public void SetNext(Node<T> next)
        {
            this.next = next;
        }
    }
}

