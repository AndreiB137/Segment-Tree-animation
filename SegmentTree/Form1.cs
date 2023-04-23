using System.Configuration;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Net.NetworkInformation;
using System.Numerics;
using Timer = System.Windows.Forms.Timer;

namespace SegmentTree
{
    struct segtree
    {
        public int sum, cmmdc, max, min;
        public int lazy;
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Font font = new Font(new FontFamily("Times New Roman"), 25);
        SolidBrush s = new SolidBrush(Color.Orange);
        Graphics G;
        Pen P = new Pen(Color.Red, 3); // nodes + vector
        Pen P2 = new Pen(Color.Lime, 3); // lines
        Pen P3 = new Pen(Color.Blue, 3); // nodes when update or query
        Pen P4 = new Pen(Color.Gold, 3); // nodes when showing the updates, queries (nodes) or the updated interval in vector
        List<PointF> nodes = new List<PointF>();
        List<Vertex> ver = new List<Vertex>();
        int[] nd;
        int[] idxNode;
        PointF[] LeftMostPoint = new PointF[15];
        PointF[] father;
        int[] cnt = new int[15];
        int[] cnt2 = new int[15];
        float[] delta = new float[15];
        float width = 80;
        float height = 80;
        int i = 0;
        int j = 0;
        int k = 0;
        int z = 0;
        float theta1, theta2;
        segtree[] Segtree = new segtree[105];
        List<PointF> leaf = new List<PointF>();
        PointF start_pos = new PointF(430, 231);
        List<PointF> quer = new List<PointF>();
        PointF[] prt;
        int N = 0;


        private void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            nodes.Add(start_pos);
            timer1.Interval = 1;
            timer2.Interval = 1;
            timer3.Interval = 1;
            timer5.Interval = 1;
            timer6.Interval = 1;
            float spread = 0;
            delta[1] = 200;
            for (int i = 0; i < 15; i++)
            {
                LeftMostPoint[i] = new PointF();
            }
            start_pos.X = (arr[0].X + arr[N - 1].X) / 2;
            start_pos.Y = arr[0].Y + arr[0].Height + height;
            BuildSegTree1(1, 1, N, 0);
            BuildSegTree2(1, 1, N, start_pos, 0, spread);
            ver.Reverse();
            timer3.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            G.DrawArc(P, ver[i].Left.X, ver[i].Left.Y, width, height, 270, 10 * z);
            G.DrawArc(P, ver[i].Right.X, ver[i].Right.Y, width, height, 270, 10 * z);
            if (z == 36)
            {
                timer1.Stop();
                i++;
                timer2.Start();
                z = 0;
            }
            z++;
        }

        bool done1 = false;
        bool done2 = false;

        private void UpdateStepDraw(PointF p1, PointF p2, PointF p3, ref int step1, ref int step2, Pen X)
        {
            // p1 is node
            // p2 is left
            // p3 is right
            theta1 = (p2.Y - p1.Y - height) / (p2.X - p1.X);
            theta2 = (p3.Y - p1.Y - height) / (p3.X - p1.X);
            if (theta1 < 0)
            {
                G.DrawLine(X, p1.X + width / 2, p1.Y + height, p1.X + width / 2 - step1, p1.Y + height + Math.Abs(theta1) * step1);

                if (theta2 < 0)
                {
                    G.DrawLine(X, p1.X + width / 2, p1.Y + height, p1.X + width / 2 - step2, p1.Y + height + Math.Abs(theta2) * step2);

                }
                else
                {
                    G.DrawLine(X, p1.X + width / 2, p1.Y + height, p1.X + width / 2 + step2, p1.Y + height + Math.Abs(theta2) * step2);
                }
            }
            else
            {
                G.DrawLine(X, p1.X + width / 2, p1.Y + height, p1.X + width / 2 + step1, p1.Y + height + Math.Abs(theta1) * step1);

                if (theta2 < 0)
                {
                    G.DrawLine(X, p1.X + width / 2, p1.Y + height, p1.X + width / 2 - step2, p1.Y + height + Math.Abs(theta2) * step2);

                }
                else
                {
                    G.DrawLine(X, p1.X + width / 2, p1.Y + height, p1.X + width / 2 + step2, p1.Y + height + Math.Abs(theta2) * step2);
                }
            }
            if (p1.Y + height + Math.Abs(theta1) * step1 >= p2.Y)
            {
                step1 = 0;
                done1 = true;
            }
            if (p1.Y + height + Math.Abs(theta2) * step2 >= p3.Y)
            {
                step2 = 0;
                done2 = true;
            }
            if (!done1)
            {
                step1++;
            }
            if (!done2)
            {
                step2++;
            }
            if (done1 && done2)
            {
                step1 = 0;
                step2 = 0;
                done1 = false;
                done2 = false;
                timer2.Stop();
                timer1.Start();
            }
        }

        private void UpdateStepDrawReverse(PointF p1, PointF p2, PointF p3, ref int step1, ref int step2, Pen X)
        {
            // p1 is node
            // p2 is left
            // p3 is right
            theta1 = (p2.Y - p1.Y - height) / (p2.X - p1.X);
            theta2 = (p3.Y - p1.Y - height) / (p3.X - p1.X);
            if (theta1 < 0)
            {
                G.DrawLine(X, p2.X + width / 2, p2.Y, p2.X + width / 2 + step1, p2.Y - Math.Abs(theta1) * step1);

                if (theta2 < 0)
                {
                    G.DrawLine(X, p3.X + width / 2, p3.Y, p3.X + width / 2 + step2, p3.Y - Math.Abs(theta2) * step2);
                }
                else
                {
                    G.DrawLine(X, p3.X + width / 2, p3.Y, p3.X + width / 2 - step2, p3.Y - Math.Abs(theta2) * step2);
                }
            }
            else
            {
                G.DrawLine(X, p2.X + width / 2, p2.Y, p2.X + width / 2 - step1, p2.Y - Math.Abs(theta1) * step1);

                if (theta2 < 0)
                {
                    G.DrawLine(X, p3.X + width / 2, p3.Y, p3.X + width / 2 + step2, p3.Y - Math.Abs(theta2) * step2);
                }
                else
                {
                    G.DrawLine(X, p3.X + width / 2, p3.Y, p3.X + width / 2 - step2, p3.Y - Math.Abs(theta2) * step2);
                }
            }
            if (p1.Y + height + Math.Abs(theta1) * step1 >= p2.Y)
            {
                step1 = 0;
                done1 = true;
            }
            if (p1.Y + height + Math.Abs(theta2) * step2 >= p3.Y)
            {
                step2 = 0;
                done2 = true;
            }
            if (!done1)
            {
                step1++;
            }
            if (!done2)
            {
                step2++;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (i == ver.Count())
            {
                timer2.Stop();
                for (int i = 0; i < N; i++)
                {
                    G.DrawString(seg[i + 1].ToString(), font, s, leaf[i].X + 20, leaf[i].Y + 20);
                }
                groupBox1.Visible = true;
                label1.Visible = true;
                done1 = done2 = false;
                i = j = k = 0;
                return;
            }
            UpdateStepDraw(ver[i].Node, ver[i].Left, ver[i].Right, ref j, ref k, P);
        }

        int store;

        private void timer5_Tick(object sender, EventArgs e)
        {
            if(i == ver.Count())
            {
                timer5.Stop();
                ver.Reverse();
                G.Clear(BackColor);
                DrawWithoutAnimationArray();
                DrawWithoutAnimationTree();
                button4.Visible = true;
                return;
            }
            UpdateStepDrawReverse(ver[i].Node, ver[i].Left, ver[i].Right, ref j, ref k, P2);
            if(done1 && done2)
            {
                j = k = 0;
                done1 = done2 = false;
                timer5.Stop();
                timer6.Start();
            }
        }
        private void timer6_Tick(object sender, EventArgs e)
        {
            G.DrawArc(P2, ver[i].Node.X, ver[i].Node.Y, width, height, 270, 10 * z);
            if(z == 36)
            {
                timer6.Stop();
                if(store == 1)
                {
                    G.DrawString(Segtree[ver[i].Idnode].sum.ToString(), font, s, ver[i].Node.X + 20, ver[i].Node.Y + 20);
                }
                else if(store == 2)
                {
                    G.DrawString(Segtree[ver[i].Idnode].cmmdc.ToString(), font, s, ver[i].Node.X + 20, ver[i].Node.Y + 20);

                }
                else if(store == 3)
                {
                    G.DrawString(Segtree[ver[i].Idnode].max.ToString(), font, s, ver[i].Node.X + 20, ver[i].Node.Y + 20);

                }
                else if(store == 4)
                {

                    G.DrawString(Segtree[ver[i].Idnode].min.ToString(), font, s, ver[i].Node.X + 20, ver[i].Node.Y + 20);
                }
                timer5.Start();
                z = 0;
                i++;
            }
            z++;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            label1.Visible = false;
            button1.Visible = false;
            button4.Visible = false;
            G = CreateGraphics();
            G.SmoothingMode = SmoothingMode.HighQuality;
        }

        private void Form1_MouseHover(object sender, EventArgs e)
        {

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

        }
        private void BuildSegTree1(int node, int left, int right, int level)
        {
            if (left == right)
            {
                //MessageBox.Show("is working");
                cnt[level]++;
                idxNode[left] = node;
            }
            else
            {
                int mid = (left + right) / 2;
                cnt[level]++;
                BuildSegTree1(2 * node, left, mid, level + 1);
                BuildSegTree1(2 * node + 1, mid + 1, right, level + 1);
            }
        }


        private void BuildSegTree2(int node, int left, int right, PointF Prev_pos, int level, float spread)
        {
            nodes.Add(Prev_pos);
            prt[node] = Prev_pos;
            if (left == right)
            {
                cnt2[level]++;
                leaf.Add(Prev_pos);
                Segtree[node].sum = seg[left];
                Segtree[node].min = seg[left];
                Segtree[node].max = seg[left];
                Segtree[node].cmmdc = seg[left];
            }
            else
            {
                int mid = (left + right) / 2;
                if (delta[level + 1] == 0)
                {
                    delta[level + 1] = (delta[level] * ((float)Math.Pow(2, level) - 1) + 2 * (spread + 100)) / ((float)Math.Pow(2, level + 1) - 1);
                }
                PointF CurrPosLeft = new PointF();
                PointF CurrPosRight = new PointF();
                if (cnt2[level + 1] == 0)
                {
                    CurrPosLeft = new PointF(Prev_pos.X - 100 - spread, Prev_pos.Y + 100);
                }
                else
                {
                    CurrPosLeft = new PointF(LeftMostPoint[level + 1].X + cnt2[level + 1] * delta[level + 1], LeftMostPoint[level + 1].Y);
                }
                if (LeftMostPoint[level + 1].Y == 0)
                {
                    LeftMostPoint[level + 1] = CurrPosLeft;
                }
                cnt2[level]++;
                father[2 * node] = Prev_pos;
                father[2 * node + 1] = Prev_pos;
                BuildSegTree2(2 * node, left, mid, CurrPosLeft, level + 1, spread);
                CurrPosRight = new PointF(LeftMostPoint[level + 1].X + cnt2[level + 1] * delta[level + 1], LeftMostPoint[level + 1].Y);
                nodes.Add(Prev_pos);
                BuildSegTree2(2 * node + 1, mid + 1, right, CurrPosRight, level + 1, spread);
                Segtree[node].sum = Segtree[2 * node].sum + Segtree[2 * node + 1].sum;
                Segtree[node].cmmdc = cmmdc(Segtree[2 * node].cmmdc, Segtree[2 * node + 1].cmmdc);
                Segtree[node].max = Math.Max(Segtree[2 * node].max, Segtree[2 * node + 1].max);
                Segtree[node].min = Math.Min(Segtree[2 * node].min, Segtree[2 * node + 1].min);
                Vertex v = new Vertex(Prev_pos, CurrPosLeft, CurrPosRight, node);
                ver.Add(v);
            }
        }

        RectangleF[] arr;
        TextBox[] rtr;
        int[] seg;

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void LineAnimation(PointF start, PointF end, ref int step, Pen X)
        {
            float tangent = (end.Y - start.Y) / (end.X - start.X);
            if(tangent > 0)
            {
                G.DrawLine(X, start.X, start.Y, start.X + step, start.Y + Math.Abs(tangent) * step);
            }
            else
            {

                G.DrawLine(X, start.X, start.Y, start.X - step, start.Y + Math.Abs(tangent) * step);
            }
            if(start.Y + Math.Abs(tangent) * step >= end.Y)
            {
                done1 = true;
            }
            else
            {
                step++;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "Text|*.txt|All|*.*";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader fin = new StreamReader(openFileDialog1.FileName);
                N = int.Parse(fin.ReadLine());
                seg = new int[N + 5];
                arr = new RectangleF[N];
                arr[0] = new RectangleF(310, 50, 80, 80);
                string[] str = fin.ReadLine().Split(' ');
                for (int i = 0; i < str.Length; i++)
                {
                    seg[i + 1] = int.Parse(str[i]);
                    if (i > 0)
                    {
                        arr[i] = arr[i - 1];
                        arr[i].X = arr[i - 1].X + 80;
                    }
                }
                nd = new int[4 * N + 5];
                idxNode = new int[4 * N + 5];
            }

            father = new PointF[4 * N + 5];
            prt = new PointF[4 * N + 5];

            G.DrawRectangles(P, arr);

            for (int i = 0; i < N; i++)
            {
                G.DrawString((seg[i + 1]).ToString(), font, s, arr[i].X + 20, arr[i].Y + 20);
            }
            button1.Visible = true;
            button3.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void DrawWithoutAnimationArray()
        {
            G.DrawRectangles(P, arr);

            for (int i = 0; i < N; i++)
            {
                G.DrawString((seg[i + 1]).ToString(), font, s, arr[i].X + 20, arr[i].Y + 20);
            }
        }

        private void DrawWithoutAnimationTree()
        {
            G.DrawArc(P, ver[0].Node.X, ver[0].Node.Y, width, height, 270, 360);

            for (int f = 0; f < ver.Count(); f++)
            {

                if (store == 1)
                {
                    G.DrawString(Segtree[ver[f].Idnode].sum.ToString(), font, s, ver[f].Node.X + 20, ver[f].Node.Y + 20);
                }
                else if (store == 2)
                {
                    G.DrawString(Segtree[ver[f].Idnode].cmmdc.ToString(), font, s, ver[f].Node.X + 20, ver[f].Node.Y + 20);

                }
                else if (store == 3)
                {
                    G.DrawString(Segtree[ver[f].Idnode].max.ToString(), font, s, ver[f].Node.X + 20, ver[f].Node.Y + 20);

                }
                else if (store == 4)
                {

                    G.DrawString(Segtree[ver[f].Idnode].min.ToString(), font, s, ver[f].Node.X + 20, ver[f].Node.Y + 20);
                }
                G.DrawArc(P, ver[f].Left.X, ver[f].Left.Y, width, height, 270, 360);
                G.DrawLine(P, ver[f].Node.X + width / 2, ver[f].Node.Y + height, ver[f].Left.X + width / 2, ver[f].Left.Y);
                G.DrawLine(P, ver[f].Node.X + width / 2, ver[f].Node.Y + height, ver[f].Right.X + width / 2, ver[f].Right.Y);
                G.DrawArc(P, ver[f].Right.X, ver[f].Right.Y, width, height, 270, 360);
            }

            for (int i = 0; i < N; i++)
            {
                G.DrawString(Segtree[idxNode[i + 1]].max.ToString(), font, s, leaf[i].X + 20, leaf[i].Y + 20);
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
           
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2.Visible = false;
            radioButton3.Visible = false;
            radioButton4.Visible = false;
            store = 1;
            ver.Reverse();
            i = j = k = z = 0;
            done1 = done2 = false;
            timer5.Start();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            G.DrawArc(P, ver[0].Node.X, ver[0].Node.Y, width, height, 0, 10 * z);
            if (z == 36)
            {
                timer3.Stop();
                timer2.Start();
                z = 0;
            }
            z++;
        }

        private int cmmdc(int a, int b)
        {
            if (b == 0)
                return a;
            else
                return cmmdc(b, a % b);
        }

        PointF[] crl;
        List<PointF> special = new List<PointF>();

        private void button2_Click_1(object sender, EventArgs e)
        {
           
        }


        private void timer7_Tick(object sender, EventArgs e)
        {
            G.DrawArc(P3, quer[0].X, quer[0].Y, width, height, 270, 10 * z);
            if(z == 36)
            {
                timer7.Stop();
                timer8.Start();
                z = 0;
            }
            z++;
        }

        private void timer8_Tick(object sender, EventArgs e)
        {
            if(i == quer.Count())
            {
                timer8.Stop();
                timer10.Start();
                i = j = k = z = 0;
                return;
            }
            PointF p1 = new PointF(crl[i].X + width / 2, crl[i].Y + height);
            PointF p2 = new PointF(quer[i].X + width / 2, quer[i].Y);
            LineAnimation(p1, p2, ref j, P3);
            if(done1)
            {
                done1 = false;
                j = 0;
                timer8.Stop();
                timer9.Start();
            }
        }


        private void timer9_Tick(object sender, EventArgs e)
        {
            G.DrawArc(P3, quer[i].X, quer[i].Y, width, height, 270, 10 * z);
            if(z == 36)
            {
                timer9.Stop();
                timer8.Start();
                z = 0;
                i++;
            }
            z++;
        }

        private void timer10_Tick(object sender, EventArgs e)
        {
            for(int it = 0; it < special.Count(); it++)
            {
                G.DrawArc(P4, special[it].X, special[it].Y, width, height, 270, 10 * z);
            }
            if(z == 36)
            {
                timer10.Stop();
                if(store == 1)
                {
                    G.DrawString("Rezultatul este " + res.sum.ToString(), font, s, new PointF(25, 242));
                }
                else if(store == 2)
                {

                    G.DrawString("Rezultatul este " + res.cmmdc.ToString(), font, s, new PointF(25, 242));
                }
                else if(store == 3)
                {

                    G.DrawString("Rezultatul este " + res.max.ToString(), font, s, new PointF(25, 242));
                }
                else if(store == 4)
                {

                    G.DrawString("Rezultatul este " + res.min.ToString(), font, s, new PointF(25, 242));
                }
                Thread.Sleep(4000);
                timer11.Start();
                return;
            }
            z++;
        }

        private void UpdateInterval(int node, int left, int right, int a, int b, int x)
        {
            if (a <= left && right <= b)
            {
                Segtree[node].sum = (right - left + 1) * x;
                Segtree[node].min = x;
                Segtree[node].max = x;
                Segtree[node].cmmdc = x;
                Segtree[node].lazy = 1;
                special.Add(prt[node]);
                quer.Add(prt[node]);
                crl[quer.Count() - 1] = father[node];
                values.Add(Segtree[node]);
            }
            else
            {
                int mid = (left + right) / 2;
                quer.Add(prt[node]);
                crl[quer.Count() - 1] = father[node];
                if (Segtree[node].lazy > 0)
                {
                    Segtree[2 * node] = Segtree[node];
                    Segtree[2 * node].sum = (mid - left + 1) * Segtree[node].max;
                    Segtree[2 * node + 1] = Segtree[node];
                    Segtree[2 * node + 1].sum = (right - mid) * Segtree[node].max;
                    Segtree[node].lazy = 0;
                }
                if (a <= mid)
                    UpdateInterval(2 * node, left, mid, a, b, x);
                if (b > mid)
                    UpdateInterval(2 * node + 1, mid + 1, right, a, b, x);
                Segtree[node].sum = Segtree[2 * node].sum + Segtree[2 * node + 1].sum;
                Segtree[node].cmmdc = cmmdc(Segtree[2 * node].cmmdc, Segtree[2 * node + 1].cmmdc);
                Segtree[node].max = Math.Max(Segtree[2 * node].max, Segtree[2 * node + 1].max);
                Segtree[node].min = Math.Min(Segtree[2 * node].min, Segtree[2 * node + 1].min);
            }
        }

        segtree res;
        int ft = 0;
        int Q;
        List<Tuple<int, int, int>> queries = new List<Tuple<int, int, int>>();
        List<segtree> values = new List<segtree>();

        private void button4_Click_1(object sender, EventArgs e)
        {
            done1 = false;
            crl = new PointF[N + 5];
            timer7.Interval = 1;
            timer8.Interval = 1;
            timer9.Interval = 1;
            timer10.Interval = 1;
            timer11.Interval = 1;
            timer12.Interval = 1;
            timer13.Interval = 1;
            timer14.Interval = 1;
            timer15.Interval = 1;
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "Text|*.txt|All|*.*";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader fin = new StreamReader(openFileDialog1.FileName);
                Q = int.Parse(fin.ReadLine());
                for(int i = 0; i < Q; i++)
                {
                    string[] str = fin.ReadLine().Split(' ');
                    int op = int.Parse(str[0]);
                    int a = int.Parse(str[1]);
                    int b = int.Parse(str[2]);
                    if (op == 1)
                    {
                        queries.Add(Tuple.Create(a, b, -1));
                    }
                    else
                    {
                        int x = int.Parse(str[3]);
                        queries.Add(Tuple.Create(a, b, x));
                    }
                }
                timer11.Start();
            }
            button4.Visible = false;
        }

        private void timer11_Tick(object sender, EventArgs e)
        {
            if(ft == Q)
            {
                timer11.Stop();
                return;
            }
            timer11.Stop();
            if (queries[ft].Item3 == -1)
            {
                res.sum = 0;
                res.min = 1000000000;
                res.max = -1;
                res.cmmdc = 0;
                special.Clear();
                quer.Clear();
                Query(1, 1, N, queries[ft].Item1, queries[ft].Item2);
                G.Clear(BackColor);
                G.DrawString("Query[(" + queries[ft].Item1.ToString() + ", " + queries[ft].Item2.ToString() + ")]", font, s, new PointF(25, arr[0].Y + arr[0].Height));
                DrawWithoutAnimationArray();
                G.DrawString("Query[(" + queries[ft].Item1.ToString() + ", " + queries[ft].Item2.ToString() + ")]", font, s, new PointF(25, arr[0].Y + arr[0].Height));
                DrawWithoutAnimationTree();
                G.DrawString("Query[(" + queries[ft].Item1.ToString() + ", " + queries[ft].Item2.ToString() + ")]", font, s, new PointF(25, arr[0].Y + arr[0].Height));
                i = j = k = z = 0;
                i = 1;
                done1 = done2 = false;
                for(int g = queries[ft].Item1 - 1; g <= queries[ft].Item2 - 1; g++)
                {
                    G.DrawRectangle(P4, arr[g].X, arr[g].Y, arr[g].Width, arr[g].Height);
                }
                timer7.Start();
            }
            else
            {
                special.Clear();
                quer.Clear();
                values.Clear();
                for(int f = queries[ft].Item1; f <= queries[ft].Item2; f++)
                {
                    seg[f] = queries[ft].Item3;
                }
                G.Clear(BackColor);
                G.DrawString("Update[(" + queries[ft].Item1.ToString() + ", " + queries[ft].Item2.ToString() + ", " + queries[ft].Item3.ToString() + ")]", font, s, new PointF(25, arr[0].Y + arr[0].Height));
                DrawWithoutAnimationTree();
                G.DrawString("Update[(" + queries[ft].Item1.ToString() + ", " + queries[ft].Item2.ToString() + ", " + queries[ft].Item3.ToString() + ")]", font, s, new PointF(25, arr[0].Y + arr[0].Height));
                UpdateInterval(1, 1, N, queries[ft].Item1, queries[ft].Item2, queries[ft].Item3);
                DrawWithoutAnimationArray();
                G.DrawString("Update[(" + queries[ft].Item1.ToString() + ", " + queries[ft].Item2.ToString() + ", " + queries[ft].Item3.ToString() + ")]", font, s, new PointF(25, arr[0].Y + arr[0].Height));
                for (int g = queries[ft].Item1 - 1; g <= queries[ft].Item2 - 1; g++)
                {
                    G.DrawRectangle(P4, arr[g].X, arr[g].Y, arr[g].Width, arr[g].Height);
                }
                i = j = k = z = 0;
                done1 = done2 = false;
                i = 1;
                timer12.Start();
            }
            ft++;
        }

        private void timer12_Tick(object sender, EventArgs e)
        {
            G.DrawArc(P3, quer[0].X, quer[0].Y, width, height, 270, 10 * z);
            if(z == 36)
            {
                timer12.Stop();
                timer13.Start();
                z = 0;
            }
            z++;
        }

        private void timer13_Tick(object sender, EventArgs e)
        {
          
            if (i == quer.Count())
            {
                timer13.Stop();
                timer15.Start();
                return;
            }
            PointF p1 = new PointF(crl[i].X + width / 2, crl[i].Y + height);
            PointF p2 = new PointF(quer[i].X + width / 2, quer[i].Y);
            LineAnimation(p1, p2, ref j, P3);
            if(done1 == true)
            {
                done1 = false;
                j = 0;
                timer14.Start();
            }
        }

        private void timer14_Tick(object sender, EventArgs e)
        {
            G.DrawArc(P3, quer[i].X, quer[i].Y, width, height, 270, 10 * z);
            if(z == 36)
            {
                timer14.Stop();
                timer13.Start();
                i++;
                z = 0;
            }
            z++;
        }

        private void timer15_Tick(object sender, EventArgs e)
        {
            for(int i = 0; i < special.Count(); i++)
            {
                G.DrawArc(P4, special[i].X, special[i].Y, width, height, 90, 10 * z);
            }
            if(z == 36)
            {
                timer15.Stop();
                G.Clear(BackColor);
                G.DrawString("Update[(" + queries[ft - 1].Item1.ToString() + ", " + queries[ft - 1].Item2.ToString() + ", " + queries[ft - 1].Item3.ToString() + ")]", font, s, new PointF(25, arr[0].Y + arr[0].Height));
                DrawWithoutAnimationArray();
                G.DrawString("Update[(" + queries[ft - 1].Item1.ToString() + ", " + queries[ft - 1].Item2.ToString() + ", " + queries[ft - 1].Item3.ToString() + ")]", font, s, new PointF(25, arr[0].Y + arr[0].Height));
                for (int g = queries[ft - 1].Item1 - 1; g <= queries[ft - 1].Item2 - 1; g++)
                {
                    G.DrawRectangle(P4, arr[g].X, arr[g].Y, arr[g].Width, arr[g].Height);
                }
                DrawWithoutAnimationTree();
                G.DrawString("Update[(" + queries[ft - 1].Item1.ToString() + ", " + queries[ft - 1].Item2.ToString() + ", " + queries[ft - 1].Item3.ToString() + ")]", font, s, new PointF(25, arr[0].Y + arr[0].Height));
                G.DrawArc(P3, quer[0].X, quer[0].Y, width, height, 270, 360);
                for (int f = 1; f < i; f++)
                {
                    G.DrawArc(P3, quer[f].X, quer[f].Y, width, height, 270, 360);
                    G.DrawLine(P3, crl[f].X + width / 2, crl[f].Y + height, quer[f].X + width / 2, quer[f].Y);
                }
                for (int i = 0; i < special.Count(); i++)
                {
                    G.DrawArc(P4, special[i].X, special[i].Y, width, height, 270, 360);
                }
                Thread.Sleep(4000);
                timer11.Start();
                z = 0;
            }
            z++;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Visible = false;
            radioButton3.Visible = false;
            radioButton4.Visible = false;
            store = 2;
            ver.Reverse();
            i = j = k = z = 0;
            done1 = done2 = false;
            timer5.Start();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Visible = false;
            radioButton2.Visible = false;
            radioButton4.Visible = false;
            store = 3;
            ver.Reverse();
            i = j = k = z = 0;
            done1 = done2 = false;
            timer5.Start();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Visible = false;
            radioButton2.Visible = false;
            radioButton3.Visible = false;
            store = 4;
            ver.Reverse();
            i = j = k = z = 0;
            done1 = done2 = false;
            timer5.Start();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Query(int node, int left, int right, int a, int b)
        {
            if (a <= left && right <= b)
            {
                special.Add(prt[node]);
                quer.Add(prt[node]);
                crl[quer.Count() - 1] = father[node];
                res.sum += Segtree[node].sum;
                res.min = Math.Min(res.min, Segtree[node].min);
                res.max = Math.Max(res.max, Segtree[node].max);
                res.cmmdc = cmmdc(Segtree[node].cmmdc, res.cmmdc);
            }
            else
            {
                int mid = (left + right) / 2;
                if (Segtree[node].lazy > 0)
                {
                    Segtree[2 * node] = Segtree[node];
                    Segtree[2 * node + 1] = Segtree[node];
                    Segtree[node].lazy = 0;
                }
                quer.Add(prt[node]);
                crl[quer.Count() - 1] = father[node];
                if (a <= mid)
                    Query(2 * node, left, mid, a, b);
                if (b > mid)
                    Query(2 * node + 1, mid + 1, right, a, b);
                Segtree[node].sum = Segtree[2 * node].sum + Segtree[2 * node + 1].sum;
                Segtree[node].cmmdc = cmmdc(Segtree[2 * node].cmmdc, Segtree[2 * node + 1].cmmdc);
                Segtree[node].max = Math.Max(Segtree[2 * node].max, Segtree[2 * node + 1].max);
                Segtree[node].min = Math.Min(Segtree[2 * node].min, Segtree[2 * node + 1].min);
            }
        }

    }

   

    class Vertex
    {
        private PointF node;
        private PointF left;
        private PointF right;
        private int idnode;

        public PointF Left
        {
            get { return left; }
            set { left = value; }
        }

        public PointF Right
        {
            get { return right; }
            set { left = value; }
        }

        public PointF Node
        {
            get { return node; }
            set { node = value; }
        }

        public int Idnode
        {
            get { return idnode; }
            set { idnode = value; }
        }

        public Vertex(PointF node, PointF left, PointF right, int idnode)
        {
            this.node = node;
            this.left = left;
            this.right = right;
            this.idnode = idnode;
        }

        public Vertex()
        {
            this.node = PointF.Empty;
            this.left = PointF.Empty;
            this.right = PointF.Empty;
        }

    }

}