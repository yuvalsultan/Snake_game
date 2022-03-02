using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        enum directions
        {
            right, left, up, down, space
        }

        directions Dir;
        Node<Rectangle> Snake;//הנחש המקורי, הראש של שרשרת החוליות נמצא בסוף הנחש
        Node<Rectangle> temp;//זמני לשם הצגת הנחש
        Node<Rectangle> TempMine;

        Node<Rectangle> apple = new Node<Rectangle>(new Rectangle()); //ריבוע יחיד של תפוח

        Node<Rectangle> mine = new Node<Rectangle>(new Rectangle()); //מוקשים
        Node<Rectangle> blow = new Node<Rectangle>(new Rectangle()); //ריבוע קסם ירוק שמעלים את המוקשים

        int RightBound; //גבול צד ימין
        int lowerBound; //גבול תחתון
        int CountMines = 0;
        List<Point> TakenP = new List<Point>();//רשימת המכילה את נקודות הנחש, מתעדנת כל אכילה של תפוח
                                               // כדי שלפני כל הגרלת תפוח חדש הוא לא יוגרל על הנחש

        int sum = 0; //כמה תפוחים אכלתי
        bool help; //משתנה עזר בולאני
        public Form1()
        {
            InitializeComponent();

            RightBound = ClientSize.Width;
            lowerBound = ClientSize.Height;

            StartGame();
            
        }
        private void StartGame() //התחלת משחק על ידי הקשת רווח
        {
            timer1.Interval = 200;
            help = true;
            start.Show();
            mine = null;
            Snake = new Node<Rectangle>(new Rectangle(50, 50, 10, 10)); //הגדרת נחש התחלתי
            Snake.SetNext(new Node<Rectangle>(new Rectangle(60, 50, 10, 10)));
            Snake.GetNext().SetNext(new Node<Rectangle>(new Rectangle(70, 50, 10, 10)));

            TakenP.Add(new Point(50, 50)); //הוספה לרשימה של נקודות הנחש
            TakenP.Add(new Point(60, 50));
            TakenP.Add(new Point(70, 50));
            apples();
        }
        private void apples() //מגריל מיקום תפוח, מוקשים, וריבועי קסם
        {
            Random rnd = new Random();

            int RndX = rnd.Next(20, RightBound - 10);
            int RndY = rnd.Next(20, lowerBound - 10);

            Point app = new Point(RndX, RndY); //מכניס את המיקום לנקודה


            while ((RndX % 10 != 0) || (RndY % 10 != 0) || (TakenP.Contains(app))) //הנחש זז בקפיצות של 10 על הגרף
            {                                              // כדי שהנחש תמיד יהיה מקביל לתפוח, התפוח צריך להיות במיקום המתחלק ב10

                // אם התפוח על הנחש, או שהמיקומים לא מתחלקים ב10, תגריל שוב
                RndX = rnd.Next(20, RightBound - 10);
                RndY = rnd.Next(20, lowerBound - 10);
                app = new Point(RndX, RndY);
            }
            apple = new Node<Rectangle>(new Rectangle(RndX, RndY, 10, 10)); //הגרלה מוצלחת

            if (int.Parse(points.Text) % 3 == 0) //מגריל מוקש כל 3 תפוחים
            {
                int RndX1 = rnd.Next(20, RightBound - 10);
                int RndY1 = rnd.Next(20, lowerBound - 10);

                Point m = new Point(RndX1, RndY1);
                while ((RndX1 % 10 != 0) || (RndY1 % 10 != 0) || (TakenP.Contains(m))) 
                {       
                    RndX1 = rnd.Next(20, RightBound - 10);
                    RndY1 = rnd.Next(20, lowerBound - 10);
                    m = new Point(RndX1, RndY1);
                }
                Node<Rectangle> nmine = new Node<Rectangle>(new Rectangle(RndX1, RndY1, 10, 10), mine);
                mine = nmine;
                CountMines++;
            }

            int num= rnd.Next(1, 10); //מגריל ריבועי קסם אקראית
            if(num==1)
            {
                int RndX2 = rnd.Next(20, RightBound - 10);
                int RndY2 = rnd.Next(20, lowerBound - 10);
                Point b = new Point(RndX2, RndY2);
                while ((RndX2 % 10 != 0) || (RndY2 % 10 != 0) || (TakenP.Contains(b))) 
                {                                             
                    RndX2 = rnd.Next(20, RightBound - 10);
                    RndY2 = rnd.Next(20, lowerBound - 10);
                    b = new Point(RndX2, RndY2);
                }
                blow = new Node<Rectangle>(new Rectangle(RndX2, RndY2, 10, 10));
            }
            TakenP.Clear(); 
        }

        private void Form1_Paint_1(object sender, PaintEventArgs e) //(נחש,תפוח) form פעולה הצובעת את המלבנים המוגדרים ב 
        {
            temp = Snake;
            TempMine = mine;
            while (temp != null)
            {
                e.Graphics.FillRectangle(Brushes.Purple, temp.GetInfo());
                temp = temp.GetNext();
            }
            temp = Snake;

            while(TempMine!=null)
            {
                e.Graphics.FillRectangle(Brushes.Red, TempMine.GetInfo());
                TempMine = TempMine.GetNext();
            }
            TempMine = mine;
            e.Graphics.FillRectangle(Brushes.Purple, apple.GetInfo());
                e.Graphics.FillRectangle(Brushes.LightGreen, blow.GetInfo());
            
        }
        private void count() //התוכנית נשלחת לכאן לאחר כל יצירת מלבן חדש בנחש
        {                    //סופרת את מספר המלבנים המרכיבים את הנחש ומציגה את מספר התפוחים האכולים
                             //בנוסף, משנה את מהירות הנחש ככל שאוכלים תפוחים
            Node<Rectangle> counter = Snake;
            while (counter != null)
            {
                sum++;
                counter = counter.GetNext();
            }
            if ((sum % 2 == 0) && (timer1.Interval > 50)) //כל אכילת 2 תפוחים, עד שהוא מגיע לגבול מסוים
                timer1.Interval = timer1.Interval - 10;

            if ((sum % 10 == 0) && (timer1.Interval < 50) && (timer1.Interval > 10)) // אחרי שעבר את הגבול, עלייה איטית יותר
                timer1.Interval = timer1.Interval - 10;

            points.Text = (sum - 3).ToString();

            sum = 0;
        }
        private void Create() // התוכנית נשלחת לכאן לאחר אכילת תפוח ומוסיפה מלבן חדש לנחש 
        {                     //כאמור ראש שרשרת החוליות היא בזנב הנחש
                              //ההוספה מתחרשת בסוף הנחש, אך בפועל מוסיפים חוליה לראש השרשרת 

            switch (Dir)  // הדבר תלוי במיקום הנחש
            {
                case directions.right: // אם הוא זז ימינה מוסיפה בצד שמאל שלו 
                    {
                        //שלה זה ראש הנחש nextיוצרים חוליה חדשה, במיקום הרצוי,כשה 
                        Node<Rectangle> first = new Node<Rectangle>(new Rectangle(Snake.GetInfo().X - 10, Snake.GetInfo().Y, 10, 10), Snake);
                        Snake = first; //תמיד נשאר בראש snake

                        count(); //שליחה לספירה
                    }
                    break;
                case directions.left: // אם הוא זז שמאלה מוסיפה בצד ימין שלו
                    {
                        Node<Rectangle> first = new Node<Rectangle>(new Rectangle(Snake.GetInfo().X + 10, Snake.GetInfo().Y, 10, 10), Snake);
                        Snake = first;
                        count(); //שליחה לספירה
                    }
                    break;
                case directions.up:// אם הוא זז למעלה מוסיפה מתחת
                    {
                        Node<Rectangle> first = new Node<Rectangle>(new Rectangle(Snake.GetInfo().X, Snake.GetInfo().Y - 10, 10, 10), Snake);
                        Snake = first;
                        count(); //שליחה לספירה
                    }
                    break;
                case directions.down:// אם הוא זז למטה מוסיפה למעלה
                    {
                        Node<Rectangle> first = new Node<Rectangle>(new Rectangle(Snake.GetInfo().X, Snake.GetInfo().Y + 10, 10, 10), Snake);
                        Snake = first;
                        count(); //שליחה לספירה
                    }
                    break;
            }
        }

        private void Eaten() //בדיקה האם התפוח נאכל
        {                    //כאשר מיקום ראש הנחש (חוליה אחרונה) מצטלב עם מיקום התפוח 
                             //בודק גם האם נאכל ריבוע קסם, אם כן נעלמים המוקשים
            Node<Rectangle> temp = Snake;

            while (temp.GetNext() != null)
            {
                temp = temp.GetNext();
            }
            
            if (temp.GetInfo().Location == apple.GetInfo().Location)
            {
                Create(); //אם נאכל שלח להוספת מלבן לנחש
                temp = Snake; 

                TakenP.Add(new Point(temp.GetInfo().X, temp.GetInfo().Y)); //לאחר שהנחש גדל מכניסים את מיקומי הנחש לרשימה
                while (temp.GetNext() != null)
                {
                    temp = temp.GetNext();
                    TakenP.Add(new Point(temp.GetInfo().X, temp.GetInfo().Y));
                }

                    apples(); //ושולחים להגרלת תפוח חדש
            }
            if(temp.GetInfo().Location==blow.GetInfo().Location) //בדיקת הצטלבות עם ריבוע קסם
            {
                mine = null;
                CountMines = 0;
                blow = new Node<Rectangle>(new Rectangle());
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) //מחליט על ערך הכיוון כאשר מקש נלחץ
        {
           
            switch (e.KeyCode)
            {
                case Keys.Space: // מקש רווח יכול להילחץ רק כאשר השחקן רוצה להתחיל את המשחק
                    {
                        if (help) // לכן נעזרתי במשתנה עזר בוליאני
                        {
                            start.Hide(); 
                            Dir = directions.right; //לאחר התחלת המשחק הנחש מתחיל לזוז ימינה
                            help = false; ;
                            timer1.Enabled = true; //מתחיל את הטיימר
                        }
                    }
                    break;

                case Keys.Right:                            // נשלח לפעולה בוליאנית שבודקת האם זה הכיוון הנגדי לכיוון הנלחץ
                    if (CantRevarse(Snake,directions.left)) //כשאני לוחצת ימינה, זה נשלח לפעולה ובודק האם הנחש לצד שמאל
                        Dir = directions.right;             
                    break;
                case Keys.Left:
                    if (CantRevarse(Snake, directions.right))
                        Dir = directions.left;
                    break;
                case Keys.Up:
                    if (CantRevarse(Snake, directions.up))
                        Dir = directions.up;
                    break;
                case Keys.Down:
                    if (CantRevarse(Snake, directions.down))
                        Dir = directions.down;
                    break;
            }
        }
        private bool CantRevarse(Node<Rectangle>a,directions where) //פעולה הבודקת האם ראש הנחש צמוד לצוואר שלו בהתאם לכיוון 
        {                                                           //כך זה מונע לחיצה לכיוון הנגדי 
        
            Node<Rectangle> temp1 = a;         //ראש הנחש                     
            Node<Rectangle> temp2 = a;         //מלבן אחד לפני

            while (temp1.GetNext() != null)
                temp1 = temp1.GetNext();

            while (temp2.GetNext().GetNext() != null)
                temp2 = temp2.GetNext();
            
            switch (where)
            {
                case directions.right: // X כשהכיוון ימינה הצוואר של הנחש יהיה מצד ימין של ציר ה
                    if ((temp1.GetInfo().X == temp2.GetInfo().X + 10) && (temp1.GetInfo().Y == temp2.GetInfo().Y)) //בדיקה האם הם צמודים
                        return false;

                    break;
                case directions.left:
                    if ((temp1.GetInfo().X + 10 == temp2.GetInfo().X) && (temp1.GetInfo().Y == temp2.GetInfo().Y))
                        return false;
                    break;

                case directions.down:
                    if ((temp1.GetInfo().X == temp2.GetInfo().X) && (temp1.GetInfo().Y + 10 == temp2.GetInfo().Y))
                        return false;
                    break;

                case directions.up:
                    if ((temp1.GetInfo().X == temp2.GetInfo().X) && (temp1.GetInfo().Y == temp2.GetInfo().Y + 10))
                        return false;
                    break;
            }
            return true;
        }
        private void timer1_Tick(object sender, EventArgs e) //interval פעולת הטיימר, כל פעם שמגיע ל     
        {
            switch (Dir) // נקבע על פי המקש הנלחץ Dir 
            {
                case directions.right:
                    //לפני כל תזוזה של נחש הוא בודק האם נגע בגבולות או האם התנגש בעצמו
                    
                    if ((TouchingBounderies(Snake, lowerBound, RightBound, Dir)) && (CrushItSelf(Snake))&&(CrushMine())) //אם הכל תקין
                    {
                        Snake = MoveItRight(Snake); // שולח את הנחש לפעולת תזוזה ימינה
                        Eaten();                    //בדיקה האם תפוח נאכל
                    }
                    else //אם לא משחק מתחיל מחדש
                    {  
                        timer1.Enabled = false;
                        MessageBox.Show("oops, You won " + points.Text + " points");
                        points.Text = 0.ToString();
                        StartGame();
                    }

                    break;

                case directions.left:
                    //Dirs.Insert(Dir);
                    if ((TouchingBounderies(Snake, lowerBound, RightBound, Dir)) && (CrushItSelf(Snake))&&(CrushMine()))
                    {
                        Snake = MoveItLeft(Snake); //תזוזה שמאלה
                        Eaten();
                    }
                    else
                    {
                        timer1.Enabled = false;
                        MessageBox.Show("oops, You won " + points.Text + " points");
                        points.Text = 0.ToString();
                        StartGame();
                    }
                    break;

                case directions.down:
                    if ((TouchingBounderies(Snake, lowerBound, RightBound, Dir)) && (CrushItSelf(Snake))&&(CrushMine()))
                    {
                        Snake = MoveItDown(Snake); //תזוזה למטה
                        Eaten();
                    }
                    else
                    {
                        timer1.Enabled = false;
                        MessageBox.Show("oops, You won " + points.Text + " points");
                        points.Text = 0.ToString();
                        StartGame();
                    }
                    break;

                case directions.up:
                    if ((TouchingBounderies(Snake, lowerBound, RightBound, Dir)) && (CrushItSelf(Snake))&&(CrushMine()))
                    {
                        Snake = MoveItUp(Snake); //תזוזה למעלה
                        Eaten();
                    }
                    else
                    {
                        timer1.Enabled = false;
                        MessageBox.Show("oops, You won " + points.Text + " points");
                        points.Text = 0.ToString();
                        StartGame();
                    }
                    break;
            }
            Invalidate();
        }

        private static Node<Rectangle> MoveItDown(Node<Rectangle>a) //תזוזה למטה
        {                                                           //פעולה זו מקבלת את הנחש, מעבירה את הזנב של הנחש להיות בראש
                                                                    //ומשנה את המיקום של הראש להיות מתחת לראש הקודם
            
                Node<Rectangle> NewA = a;     //יצירת משתנה עזר                          
                a = NewA.GetNext();           //מעבירה את המקור לחוליה השנייה                      
                Node<Rectangle> temp = NewA.GetNext(); ; //עוד משתנה עזר הנועד לריצה על השרשרת

                NewA.SetNext(null); //ניתוק הראש מהשרשרת
                while (temp.GetNext() != null) //ריצה על השרשרת עד שמגיעה לסוף
                    temp = temp.GetNext();

                temp.SetNext(NewA); //מוסיפה לסוף השרשרת את החוליה הראשונה
                NewA.SetInfo(new Rectangle(temp.GetInfo().X, temp.GetInfo().Y + 10, 10, 10)); //משנה את המיקום 
                 // 
           
            return a;
        }
        private static Node<Rectangle> MoveItUp(Node<Rectangle>a) //תזוזה למעלה
        {
            Node<Rectangle> NewA = a;
            a = NewA.GetNext();
            Node<Rectangle> temp = NewA.GetNext(); ;
            NewA.SetNext(null);
            while (temp.GetNext() != null)
                temp = temp.GetNext();
            temp.SetNext(NewA);
            NewA.SetInfo(new Rectangle(temp.GetInfo().X, temp.GetInfo().Y - 10, 10, 10));
            return a;
        }
        private static Node<Rectangle> MoveItRight(Node<Rectangle> a)//תזוזה ימינה 
        {
            Node<Rectangle> NewA = a;
            a = NewA.GetNext();
            Node<Rectangle> temp = NewA.GetNext(); ;
            NewA.SetNext(null);
            while (temp.GetNext() != null)
                temp = temp.GetNext();
            temp.SetNext(NewA);
            NewA.SetInfo(new Rectangle(temp.GetInfo().X+10, temp.GetInfo().Y, 10, 10));
            return a;
        }

        private static Node<Rectangle> MoveItLeft(Node<Rectangle> a)//תזוזה שמאלה
        {
            Node<Rectangle> NewA = a;
            a = NewA.GetNext();
            Node<Rectangle> temp = NewA.GetNext(); ;
            NewA.SetNext(null);
            while (temp.GetNext() != null)
                temp = temp.GetNext();
            temp.SetNext(NewA);
            NewA.SetInfo(new Rectangle(temp.GetInfo().X - 10, temp.GetInfo().Y, 10, 10));
            return a;
        }
        private static bool CrushItSelf(Node<Rectangle> a) //מקבל את הנחש 
        {                                                  // מחזיר שקר אם לראש הנחש(חוליה אחרונה) יש נקודה משותפת עם אחד המלבנים בנחש 
                                                           //כלומר עם הנחש התנגש בעצמו
                                                           //אמת אם אין

            Node<Rectangle> temp=a; //משתנה עזר כדי לרוץ על הנחש
            Node<Rectangle> last=a; //משתנה עזר כדי להגיע לראש הנחש, חוליה אחרונה

            while (last.GetNext() != null) 
                last = last.GetNext(); 

            Point crush1, crush2;
            crush1 = new Point(last.GetInfo().X, last.GetInfo().Y); // נקודה ראשונה היא הנקודה של ראש הנחש
            
            while (temp.GetNext() != null)
            {
                crush2 = new Point(temp.GetInfo().X, temp.GetInfo().Y); //ריצה על הנחש ושמירת נקודה רלוונטית
                if (crush1 == crush2) 
                    return false;
                else
                    temp = temp.GetNext();
            }
            return true;
        }
        private bool CrushMine() //התנגשות נחש במוקש 
        {
            Node<Rectangle> last = Snake;
            Node<Rectangle> tempMine = mine;

            while (last.GetNext() != null)
                last = last.GetNext();

            while (tempMine != null)
            {
                if (tempMine.GetInfo().Location == last.GetInfo().Location)
                    return false;
                tempMine = tempMine.GetNext();
            }
            return true;
        }
        private static bool TouchingBounderies(Node<Rectangle>a,int low,int right,directions dir1) //מקבל את הנחש, הגבולות, והכיוון
        {
            Node<Rectangle> Bounds; //משתנה עזר כדי להגיע לראש הנחש
            Bounds = a;
            while (Bounds.GetNext() != null)
                Bounds = Bounds.GetNext();
            
            switch (dir1) //תלוי כיוון בודק האם הנקודה של ראש הנחש שווה לגבול
            {
                case (directions.right):
                    if (right <= Bounds.GetInfo().X+10)
                        return false;
                    break;

                case directions.down:
                    if (low <=Bounds.GetInfo().Y+10)
                    return false;
                    break;
                case directions.up:
                    if (Bounds.GetInfo().Y <=0)
                        return false;
                    break;
                case directions.left:
                    if (Bounds.GetInfo().X ==0)
                        return false;
                    break;
            }
            return true;
                

        }
    }
}
