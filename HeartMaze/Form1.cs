using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace HeartMaze
{
    public partial class HeartMaze : Form
    {
        public HeartMaze()
        {
            InitializeComponent();
        }

        private void HeartMaze_Load(object sender, EventArgs e)
        {
            initArea();
            InitUI();
        }

        public class coordinate //坐标
        {
            public int i;
            public int j;
            public int dir; // 方向
            public coordinate(int n, int m)
            {
                i = n;
                j = m;
            }
            public coordinate(int n, int m, int Dir)
            {
                i = n;
                j = m;
                dir = Dir;
            }
        }

        const int N = 37;
        const int M = 47;

        enum Dir
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
        }

        enum Maze
        {
            ROAD,       // 通路
            WALL,       // 墙
            BORDER,     // 边界
            OUTHEART    // 心外
        };
        int[,] maze = new int[N, M]; // 取值为以上枚举类型

        bool[,] area = new bool[N, M];      // 心形区域
        bool[,] visited = new bool[N,M];    // 是否访问过

        List<coordinate> WallList = new List<coordinate>();  // 未访问的邻墙列表
        List<coordinate> ExitList = new List<coordinate>();  // 出口列表
        List<coordinate> BorderList = new List<coordinate>();// 边界列表

        coordinate entry = new coordinate(1, 1); // 入口位置
        coordinate exit;                         // 出口位置，迷宫生成后在出口列表中随机选取

        coordinate walls;
        Button[,] btns;

        Random rnd = new Random();

        /* 生成迷宫：随机Prim*/
        private void GenerateMaze(object sender, EventArgs arg)
        {
            if (WallList.Count != 0)
            {
                int index;
                int i = 0;
                int j = 0;
                coordinate wall;
                do
                {
                    index = rnd.Next(WallList.Count);

                    if (WallList.Count != 0)
                    {
                        wall = WallList[index];
                        i = wall.i;
                        j = wall.j;
                        visited[i, j] = true;

                        if (RoadCount(i, j) == 0)
                        {
                            WallList.RemoveAt(index);
                        }
                    }
                    else return;
                } while (RoadCount(i, j) == 0);

                WallList.RemoveAt(index);
                maze[i, j] = (int)Maze.ROAD;
                RefreshUI(i, j);
                AddWall(i, j);
                for (int q = 0; q < 15000000; q++) ;
            }
            else
            {
                //Application.Idle -= new EventHandler(GenerateMaze);
                //Application.Idle += new EventHandler(FillUI);
                FillUI();

            }
        }

        /* (i,j)附近的通路统计 */
        private int RoadCount(int i, int j)
        {
            int count_i = 0;
            int count_j = 0;
            int count = 0;
            if (i + 1 < N && j + 1 < M)
            {
                if (maze[i + 1, j] == 0) count_i++;
                if (maze[i - 1, j] == 0) count_i++;
                if (maze[i, j + 1] == 0) count_j++;
                if (maze[i, j - 1] == 0) count_j++;
                if (maze[i + 1, j + 1] == 0) count++;
                if (maze[i - 1, j + 1] == 0) count++;
                if (maze[i + 1, j - 1] == 0) count++;
                if (maze[i - 1, j - 1] == 0) count++;

                if (count_i == 1 && count_j == 0 && count <= 1) return 1;
                if (count_j == 1 && count_i == 0 && count <= 1) return 2;
            }
            return 0;
        }

        /* 添加邻墙 */
        private void AddWall(int i, int j)
        {
            if (i >= 1 && i <= N - 2 && j >= 1 && j <= M - 2)
            {
                if (visited[i + 1, j] == false && maze[i + 1, j] == 1)
                {
                    walls = new coordinate(i + 1, j);
                    WallList.Add(walls);
                }
                if (visited[i - 1, j] == false && maze[i - 1, j] == 1)
                {
                    walls = new coordinate(i - 1, j);
                    WallList.Add(walls);
                }
                if (visited[i, j - 1] == false && maze[i, j - 1] == 1)
                {
                    walls = new coordinate(i, j - 1);
                    WallList.Add(walls);
                }
                if (visited[i, j + 1] == false && maze[i, j + 1] == 1)
                {
                    walls = new coordinate(i, j + 1);
                    WallList.Add(walls);
                }
            }

            for (int n = 0; n < WallList.Count; n++)
                for (int m = n + 1; m < WallList.Count; m++)
                {
                    if (WallList[n].i == WallList[m].i && WallList[n].j == WallList[m].j)
                    {
                        WallList.RemoveAt(m);
                        break;
                    }
                }

        }

        /* 初始化界面 */
        private void InitUI()
        {
            btns = new Button[N, M];
            int x = 1, y = 1, w = 15, d = w;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    visited[i, j] = false;

                    Button btn = new Button();
                    btn.Left = x + j * d;
                    btn.Top = y + i * d;
                    btn.Width = w;
                    btn.Height = w;

                    btn.BackColor = Color.Red;

                    if (area[i, j])
                    {
                        if (i == 0 || i == N - 1 || j == 0 || j == M - 1)
                        {
                            maze[i, j] = (int)Maze.BORDER;

                        }
                        else if (!area[i, j - 1] || !area[i, j + 1] || !area[i - 1, j] || !area[i + 1, j])
                        {
                            maze[i, j] = (int)Maze.BORDER;
                        }
                        else
                        {
                            maze[i, j] = (int)Maze.WALL;
                        }

                        if (maze[i, j] == (int)Maze.BORDER)
                        {
                            coordinate temp = new coordinate(i, j);
                            BorderList.Add(temp);
                        }
                    }
                    else
                    {
                        maze[i, j] = (int)Maze.OUTHEART;
                    }

                    btn.Visible = false;

                    btns[i, j] = btn;
                    this.pnlBoard.Controls.Add(btns[i, j]);



                }
            }

            /* 设置入口 */
            maze[entry.i, entry.j] = (int)Maze.ROAD;
            RefreshUI(entry.i, entry.j);
            visited[entry.i, entry.j] = true;

            walls = new coordinate(entry.i, entry.j + 1);
            WallList.Add(walls);
            walls = new coordinate(entry.i + 1, entry.j);
            WallList.Add(walls);


            /* 全部不可见 */
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    btns[i, j].Visible = false;
                }
            }
        }

        /* 刷新(x,y)及其附近区域*/
        private void RefreshUI(int x, int y)
        {
            for (int i = x-1; i < x+2; i++)
            {
                for (int j = y-1; j < y+2; j++)
                {
                    if (area[i, j])
                    {
                        if (maze[i, j] == (int)Maze.WALL)
                        {
                            btns[i, j].Visible = true;
                        }
                        else
                        {
                            btns[i, j].Visible = false;

                        }
                    }
                    else
                    {
                        btns[i, j].Visible = false;
                    }

                }
            }
        }

        int index = 0;
        int state = 1;
        /* 迷宫生成后填补空格*/
        private void FillUI()//object sender, EventArgs arg)
        {

            if (state == 2)
            {
                findRoad();
                return;
            }
            if (index <BorderList.Count())
            {
                if (BorderList[index].i == 0 || BorderList[index].i == N - 1 || BorderList[index].j == 0 || BorderList[index].j == M - 1)
                {
                    int count = 0;
                    if (BorderList[index].i == 0 && maze[BorderList[index].i + 1, BorderList[index].j] == (int)Maze.ROAD)
                    {
                        count = 1;
                    }
                    if (BorderList[index].i == N - 1 && maze[BorderList[index].i - 1, BorderList[index].j] == (int)Maze.ROAD)
                    {
                        count = 1;
                    }
                    if (BorderList[index].j == 0 && maze[BorderList[index].i, BorderList[index].j + 1] == (int)Maze.ROAD)
                    {
                        count = 1;
                    }
                    if (BorderList[index].j == M - 1 && maze[BorderList[index].i, BorderList[index].j - 1] == (int)Maze.ROAD)
                    {
                        count = 1;
                    }

                    if (count == 1) 
                    {
                        if (BorderList[index].i > N / 2 && BorderList[index].j > M / 2)  //限定出口位置的范围
                        {
                            coordinate temp = new coordinate(BorderList[index].i, BorderList[index].j);
                            ExitList.Add(temp);

                        }
                    }
                }
                btns[BorderList[index].i, BorderList[index].j].Visible = true;
                index++;
                for (int q = 0; q < 1000000; q++) ;
            }
            else
            {
                if (state == 1)
                {
                    btns[entry.i - 1, entry.j].Visible = false;
                    btns[entry.i, entry.j].Visible = false;

                    //从出口列表随机选取一个位置作为出口
                    index = rnd.Next(ExitList.Count());
                    exit = new coordinate(ExitList[index].i, ExitList[index].j);
                    btns[exit.i, exit.j].Visible = false;

                    RoadList.Add(new coordinate(entry.i - 1, entry.j, (int)Dir.DOWN));
                    RoadList.Add(new coordinate(entry.i, entry.j, (int)Dir.DOWN));

                    road_visited[entry.i - 1, entry.j] = true;
                    road_visited[entry.i, entry.j] = true;
                    btns[entry.i - 1, entry.j].BackColor = Color.Green;
                    btns[entry.i - 1, entry.j].Visible = true;
                    btns[entry.i, entry.j].BackColor = Color.Green;
                    btns[entry.i, entry.j].Visible = true;


                    maze[entry.i - 1, entry.j] = (int)Maze.ROAD;
                    maze[exit.i, exit.j] = (int)Maze.ROAD;


                    //Application.Idle -= new EventHandler(FillUI);
                    //Application.Idle += new EventHandler(findRoad);
                    state = 2;
                }
                
            }
            

        }
        
        /* 初始化“70”区域 */
        void initArea()
        {
            int[,] temp = {{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,1,1,1,1,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,1,1,1,1,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,1,1,1,1,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
                        };
            for (int i=0;i<N;i++)
            {
                for (int j = 0;j<M;j++)
                {
                    
                    if (temp[i,j] ==0)
                        area[i, j] = true;
                    else
                        area[i, j] = false;
                }
            }
        }

        /* 寻找路径 */
        List<coordinate> RoadList = new List<coordinate>();
        Stack<coordinate> roads = new Stack<coordinate>();
        Stack<coordinate> CrossStack = new Stack<coordinate>();
        bool[,] road_visited = new bool[N, M];
        void findRoad()//object sender, EventArgs arg)
        {
            coordinate road = RoadList.Last();

            if(road.i == exit.i&& road.j == exit.j)
            {
                //GenerateRoads();
                //Application.Idle -= new EventHandler(findRoad);
                Application.Idle -= new EventHandler(GenerateMaze);
                return;
            }


            int count = CountRoad(road.i, road.j);

            if (count == 2)  //有两个出口
            {
                
                CrossStack.Push(road);


                AddRoad(road.i, road.j);
            }
            else if (count == 1)  //只有一个出口
            {
                AddRoad(road.i, road.j);
            }
            else if (count == 0)  //没有出口
            {
                AddRoad(CrossStack.Peek().i, CrossStack.Pop().j);
            }
            for (int q = 0; q < 15000000; q++) ;

        }

        void AddRoad(int i, int j)
        {

            if (maze[i + 1, j] == (int)Maze.ROAD && road_visited[i + 1, j] == false)
            {
                RoadList.Add(new coordinate(i + 1, j));
            }
            else if (maze[i - 1, j] == (int)Maze.ROAD && road_visited[i - 1, j] == false)
            {
                RoadList.Add(new coordinate(i - 1, j));
            }
            else if (maze[i, j + 1] == (int)Maze.ROAD && road_visited[i, j + 1] == false)
            {
                RoadList.Add(new coordinate(i, j + 1));
            }
            else if (maze[i, j - 1] == (int)Maze.ROAD && road_visited[i, j - 1] == false)
            {
                RoadList.Add(new coordinate(i, j - 1));
            }

            btns[RoadList.Last().i, RoadList.Last().j].BackColor = Color.Green;
            btns[RoadList.Last().i, RoadList.Last().j].Visible = true;
            road_visited[RoadList.Last().i, RoadList.Last().j] = true;

        }

        /* 计算通路 */
        int CountRoad(int i,int j)
        {
            int count = 0;



            if (maze[i + 1, j] == (int)Maze.ROAD && road_visited[i + 1, j] == false)
            {
                count++;
            }
            if (maze[i - 1, j] == (int)Maze.ROAD && road_visited[i - 1, j] == false)
            {
                count++;
            }
            if (maze[i, j + 1] == (int)Maze.ROAD && road_visited[i, j + 1] == false)
            {
                count++;
            }
            if (maze[i, j - 1] == (int)Maze.ROAD && road_visited[i, j - 1] == false)
            {
                count++;
            }
            return count;
        }

        void GenerateRoads()
        {
            coordinate head = new coordinate(CrossStack.Peek().i, CrossStack.Pop().j);
            coordinate end = new coordinate(CrossStack.Peek().i, CrossStack.Peek().j);

            int ii = head.i;
            int jj = head.j;

            while (ii != end.i || jj != end.j)
            {
                ii = ii + (head.i < end.i ? 1 : -1);
                jj = jj + (head.j < end.j ? 1 : -1);
                btns[ii, jj].BackColor = Color.Green;
                btns[ii, jj].Visible = true;
            }

            

        }

        private bool isProcess = true;
        private void startBtn_Click(object sender, EventArgs e)
        {

            if (isProcess)
            {
                Application.Idle += new EventHandler(GenerateMaze);
                this.startBtn.Text = "Stop";
            }
            else
            {
                Application.Idle -= new EventHandler(GenerateMaze);
                this.startBtn.Text = "Start";

            }
            isProcess = !isProcess;
        }
    }
}


