using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace MathModule
{
    /// <Triangulation>
    /// Адский пиздец на который ты сам подписался.
    /// Цель класса - создание, обработка и хранение триангуляции построенной на определённом множестве точек
    /// </Triangulation>
    public class Triangulation : MathObject
    {
        public int insideCount = 0;
        public int outshideCount = 0;
        public int EdgeCount = 0;
        public int PointCount = 0;




        /// <переменные>
        /// loopCount - переменная - счётчик для нахождения залупливания циклов (на момент написания - цикла поиска треугольника).
        /// precision - переменная используемая для установки границы округления.
        /// </переменные>
        TriangulationSearchMassive searchArray;
        int loopCount;
        int precision;


        /// <Переменные - массивы>
        /// GetPoints() - массив всех точек триангуляции, в ходе работы НИКАК не меняется и НЕ БУДЕТ ИНАЧЕ ВСЁ СЛОМАЕТСЯ.
        /// edges - массив всех рёбер (граней) триангуляции.
        /// triangles - массив всех треугольников триангуляции.
        /// outerEdges - массив всех внешних рёбер триангуляции.
        /// planar - массив планарного разбиения
        /// </Переменные - массивы>
        public List<Point> points;
        public List<TriangulationEdge> edges;
        public List<Triangle> triangles;
        public List<TriangulationEdge> outerEdges;
        public LinkCell[,] planar;
        public List<LinkedList<Point>> outerIsolines;
        public List<LinkedList<Point>> innerIsolines;



        // Конструктор класса Triangulation. Инициализирует глобальные переменные.
        public Triangulation()
        {
            loopCount = 0;

            //currentPlanarMod = 1;
            //planarMod = 1;
            //planarBorder = 4 * planarMod;
            //section = 0;


            points = new List<Point>();
            edges = new List<TriangulationEdge>();
            triangles = new List<Triangle>();
            outerEdges = new List<TriangulationEdge>();
            outerIsolines = new List<LinkedList<Point>>();
            innerIsolines = new List<LinkedList<Point>>();
        }



        /// <CreateTriangulation>
        /// Основной метод класса, используется для построения триангуляции.
        /// Работает на итеративном алгоритме по следующей логике:
        /// 1. Строится начальный треугольник.
        /// 2. Для каждой последующей точки повторяются шаги 3-5.
        /// 3. Через FindTriangle определяется либо треугольник в котором оказалась точка, либо факт её нахождения вне триангуляции.
        /// 4. При попадании точки в треугольник, он разбивается на 2 новых А СТАРЫЙ НЕ УДАЛЯЕТСЯ. При попадании вне триангуляции проверяется видимость
        /// внешних рёбер для при положении точки, после чего с каждым видимым ребром создаются новые треугольники СТАРЫЕ НИКАК НЕ ЗАДЕЙСТВОВАНЫ.
        /// 5. Проверяется условие делоне для всех созданных треугольников.
        /// </CreateTriangulation>
        /// <param name="rawPoints"> Точки по которым будет строится триангуляция, их должно быть хотя бы 3</param>
        public void CreateTriangulation(List<Point> rawPoints)
        {
            // проверка пунктов на коллинеарность (т.к. треугольник может быть построен только по 3 не коллинеарным точкам).
            if (rawPoints.Count < 3) return;
            points = rawPoints;
            for (int i = 0; IsCollinear(points[0], points[1], points[2]); i++)
            {
                if (i + 3 == points.Count)
                {
                    return;
                }
                Point transf = points[0];
                points[0] = points[3 + i];
                points[3 + i] = transf;
            }
            edges.Add(new TriangulationEdge(points[0], points[1]));
            edges.Add(new TriangulationEdge(points[1], points[2]));
            edges.Add(new TriangulationEdge(points[2], points[0]));
            outerEdges.Add(edges[0]);
            outerEdges.Add(edges[1]);
            outerEdges.Add(edges[2]);
            triangles.Add(new Triangle(edges[0], edges[1], edges[2]));


            searchArray = new TriangulationSearchMassive(this);
            try
            {
                for (int i = 3; i < points.Count; i++)
                {
                    AddPoint(points[i]);
                    if (this.GetStatus() == 1)
                    {
                        //i--;
                        //this.SetStatus(0);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }



        /// <summary>
        /// Функция для добавления точки в триангуляцию, работает почти для любого случая.
        /// </summary>
        /// <param name="point"> Точка, которая добавляется в триангуляцию. </param>
        public void AddPoint(Point point, bool isDeloneNeeded = true)
        {
            // находим треугольник ближайший к добавляемой точке.
            Triangle tr = FindTriangle(point);
            if (tr == null)
            {
                // Вызываем функцию добавления точки вне триангуляции.
                ++outshideCount;
                PointOutsideTriangulation(point, isDeloneNeeded);
                return;
            }

            // Проверяем совпадает ли точка с одной из вершин полученного треугльника если да - ничего не делаем, т.к. перепись под замену
            // займёт слишком много времени.
            if (tr.IsPointIntersect2D(point))
            {
                //points.Remove(point);
                //this.SetStatus(1);
                ++PointCount;
                return;
            }
            // Проверяем попадает ли точка на одну из граней полученного треугольнка если да - вызываем функцию разбивки и пропускаем следующие действия.
            foreach (TriangulationEdge edge in tr.edges)
            {
                if (edge.isPointBelongToSegment(point))
                {
                    ++EdgeCount;
                    PointOnEdge(edge, point, isDeloneNeeded);
                    return;
                }
            }
            // Вызываем функцию добавления точки внутри триангуляции.
            ++insideCount;
            PointInsideTriangulation(point, tr, isDeloneNeeded);
        }















































        /// <summary>
        /// Функция для добавления точки вне триангуляции, а также создания и обновления выпуклой оболочки триангуляции.
        /// </summary>
        /// <param name="point"> Точка, которая добавляется в триангуляцию. </param>
        public void PointOutsideTriangulation(Point point, bool isDeloneNeeded = true, bool addToPlanar = true)
        {
            // создаём переменные и массивы для дальнейшего использования
            List<Point> usedPoints = new List<Point>();
            List<TriangulationEdge> newEdges = new List<TriangulationEdge>();
            List<TriangulationEdge> oldOuterEdges = new List<TriangulationEdge>();
            List<Triangle> newTriangles = new List<Triangle>();
            bool isIntersecting = false;
            bool isFirstEdgeNew = false;
            bool isSecondEdgeNew = false;
            TriangulationEdge newEdge1 = null;
            TriangulationEdge newEdge2 = null;
            // Проверяем пересечение для каждого внешнего ребра 
            foreach (TriangulationEdge edge in outerEdges)
            {
                //MessageBox.Show(edge.ToString());
                // Создаём новые грани, если точки их образующие уже есть в массиве, то присваиваем им уже существующую грань
                if (!usedPoints.Contains(edge.points[0]))
                {
                    newEdge1 = new TriangulationEdge(point, edge.points[0]);
                    isFirstEdgeNew = true;
                }
                else
                {
                    foreach (TriangulationEdge newEdge in newEdges)
                    {
                        if (newEdge.IsContainingPoint(edge.points[0]))
                        {
                            newEdge1 = newEdge;
                        }
                    }
                }
                if (!usedPoints.Contains(edge.points[1]))
                {
                    newEdge2 = new TriangulationEdge(point, edge.points[1]);
                    isSecondEdgeNew = true;
                }
                else
                {
                    foreach (TriangulationEdge newEdge in newEdges)
                    {
                        if (newEdge.IsContainingPoint(edge.points[1]))
                        {
                            newEdge2 = newEdge;
                        }
                    }
                }
                // Непосредственно цикл проверки пересечения.
                foreach (TriangulationEdge otherEdge in outerEdges)
                {
                    if (edge == otherEdge)
                    {
                        continue;
                    }
                    Point middle = edge.GetMiddlePoint();
                    Point intersection = otherEdge.GetIntersection2D(point, middle);
                    //Point intersection1 = otherEdge.GetIntersection2D(point, edge.GetPoints()[0]);
                    //Point intersection2 = otherEdge.GetIntersection2D(point, edge.GetPoints()[1]);
                    if (intersection != null)
                    {
                        isFirstEdgeNew = false;
                        isSecondEdgeNew = false;
                        isIntersecting = true;
                        break;
                    }
                    //if (intersection1 != null)
                    //{
                    //    count1++;
                    //}
                    //if (intersection2 != null)
                    //{
                    //    count2++;
                    //}
                }
                //Если есть пересечение - пропустить это ребро.
                if (isIntersecting)
                {
                    isIntersecting = false;
                    continue;
                }
                //if(count1 != 1 || count2 != 1)
                //{
                //    isFirstEdgeNew = false;
                //    isSecondEdgeNew = false;
                //    continue;
                //}
                // Если первое из новых рёбер - истинно новое добавить его и точку образующую его в соответствующие массивы.
                if (isFirstEdgeNew)
                {
                    newEdges.Add(newEdge1);
                    usedPoints.Add(edge.points[0]);
                    isFirstEdgeNew = false;
                }
                // Если певторое из новых рёбер - истинно новое добавить его и точку образующую его в соответствующие массивы.
                if (isSecondEdgeNew)
                {
                    newEdges.Add(newEdge2);
                    usedPoints.Add(edge.points[1]);
                    isSecondEdgeNew = false;
                }

                // Создаём новый треугольник, засовываем его в массив треугольников, после чего добавляем старую грань.
                // в масси для старых граней
                Triangle nTr = new Triangle(newEdge1, newEdge2, edge);
                triangles.Add(nTr);
                newTriangles.Add(nTr);
                oldOuterEdges.Add(edge);
            }
            // Удаляем старые грани из массива внешних граней.
            foreach (TriangulationEdge edge in oldOuterEdges)
            {
                outerEdges.Remove(edge);
            }
            // Добавляем новые грани в массив новых граней и массив простых граней.
            foreach (TriangulationEdge edge in newEdges)
            {
                edges.Add(edge);
                if (edge.isBorderEdge())
                {
                    outerEdges.Add(edge);
                }
            }
            if (isDeloneNeeded)
            {
                foreach (Triangle testTr in newTriangles)
                {
                    TestTriangulation(testTr);
                }
            }
            if (addToPlanar)
            {
                foreach (Triangle testTr in newTriangles)
                {
                    //planar[Convert.ToInt32(Math.Truncate((testTr.GetCentroid().X - minX) / section)), Convert.ToInt32(Math.Truncate((testTr.GetCentroid().Y - minY) / section))].SetTriangle(testTr);
                    searchArray.AddTriangle(testTr);
                }
            }
        }



        /// <summary>
        /// Функция для добавления точки внутрь существующего треугольника.
        /// </summary>
        /// <param name="point"> Точка, которая добавляется в триангуляцию. </param>
        /// <param name="tr"> Треугольник, в который добавляется точка. </param>
        public void PointInsideTriangulation(Point point, Triangle tr, bool isDeloneNeeded = true, bool addToPlanar = true)
        {
            // Создаём новые рёбра и добавляем их в массив для рёбер.
            Point[] trPoints = tr.GetPoints();
            if (!tr.IsInside(point))
            {
                MessageBox.Show($"ОБнаружена точка вне треугольника, в который она записывается\n{tr}\n\n{tr.GetStatus()}\n\n {point}");
            }

            TriangulationEdge nEdge1 = new TriangulationEdge(trPoints[0], point);
            TriangulationEdge nEdge2 = new TriangulationEdge(trPoints[1], point);
            TriangulationEdge nEdge3 = new TriangulationEdge(trPoints[2], point);
            edges.Add(nEdge1);
            edges.Add(nEdge2);
            edges.Add(nEdge3);
            //Создаём новые треугольники, исключая из старых рёбер ссылки на старый треугольник.
            Triangle nTr1 = new Triangle(tr.GetEdgeByPoints(trPoints[0], trPoints[1]), nEdge1, nEdge2, tr, true);
            Triangle nTr2 = new Triangle(tr.GetEdgeByPoints(trPoints[1], trPoints[2]), nEdge2, nEdge3, tr, true);
            tr.PseudoConstruct(tr.GetEdgeByPoints(trPoints[2], trPoints[0]), nEdge3, nEdge1, tr, true);
            // Добавляем новые треугольники в массив треугольников, после чего тестируем их на Делоне.
            if (addToPlanar)
            {
                //planar[Convert.ToInt32(Math.Truncate((nTr1.GetCentroid().X - minX) / section)), Convert.ToInt32(Math.Truncate((nTr1.GetCentroid().Y - minY) / section))].SetTriangle(nTr1);
                //planar[Convert.ToInt32(Math.Truncate((nTr2.GetCentroid().X - minX) / section)), Convert.ToInt32(Math.Truncate((nTr2.GetCentroid().Y - minY) / section))].SetTriangle(nTr2);
                //planar[Convert.ToInt32(Math.Truncate((tr.GetCentroid().X - minX) / section)), Convert.ToInt32(Math.Truncate((tr.GetCentroid().Y - minY) / section))].SetTriangle(tr);

                searchArray.AddTriangle(nTr1);
                searchArray.AddTriangle(nTr2);
                searchArray.AddTriangle(tr);
            }
            if (tr.IsEqual2D(nTr1))
            {
                MessageBox.Show($"Первый треугольник\n{tr}\n\n{nTr1}");
            }
            if (tr.IsEqual2D(nTr2))
            {
                MessageBox.Show($"Второй треугольник\n{tr}\n\n{nTr2}");
            }
            if (nTr1.IsEqual2D(nTr2))
            {
                MessageBox.Show($"новые треугольники\n{nTr1}\n\n{nTr2}");
            }
            triangles.Add(nTr1);
            triangles.Add(nTr2);
            if (isDeloneNeeded)
            {
                TestTriangulation(nTr1);
                TestTriangulation(nTr2);
                TestTriangulation(tr);
            }
        }



        /// <PointOnEdge>
        /// Функция для отстраивания новых треугольников при попадании точки ровно на ребро триангуляции.
        /// 
        /// 
        /// 
        /// Перепеши под новую безотходную систему.
        /// А теперь ещё и проверь правильность.
        /// 
        /// 
        /// </PointOnEdge>
        /// <param name="edge"> Ребро, на которое попала точка. </param>
        /// <param name="point"> Точка, попавшая на ребро. </param>
        public void PointOnEdge(TriangulationEdge edge, Point point, bool isDeloneNeeded = true, bool isPlanarNeeded = true)
        {
            //MessageBox.Show("Эта хуйня здесь");
            //MessageBox.Show($"{edge}\n\n{point}");
            TriangulationEdge nEdge3 = new TriangulationEdge(edge.points[0], point);
            TriangulationEdge nEdge4 = new TriangulationEdge(edge.points[1], point);
            Triangle nTr1 = null;
            Triangle nTr2 = null;
            bool firstDelone = false;
            bool secondDelone = false;
            if (edge.triangles[0] != null)
            {
                Triangle tr1 = edge.triangles[0];
                Point p1 = tr1.GetOtherPoint(edge.points[0], edge.points[1]);
                TriangulationEdge nEdge1 = new TriangulationEdge(p1, point);
                nTr1 = new Triangle(tr1.GetEdgeByPoints(p1, edge.points[0]), nEdge1, nEdge3, tr1, true);
                tr1.PseudoConstruct(tr1.GetEdgeByPoints(p1, edge.points[1]), nEdge1, nEdge4, tr1, true);
                edges.Add(nEdge1);
                triangles.Add(nTr1);
                firstDelone = true;
            }
            if (edge.triangles[1] != null)
            {
                Triangle tr2 = edge.triangles[1];
                Point p2 = tr2.GetOtherPoint(edge.points[0], edge.points[1]);
                TriangulationEdge nEdge2 = new TriangulationEdge(p2, point);
                nTr2 = new Triangle(tr2.GetEdgeByPoints(p2, edge.points[0]), nEdge2, nEdge3, tr2, true);
                tr2.PseudoConstruct(tr2.GetEdgeByPoints(p2, edge.points[1]), nEdge2, nEdge4, tr2, true);
                edges.Add(nEdge2);
                triangles.Add(nTr2);
                secondDelone = true;
            }
            edges.Add(nEdge3);
            edges.Add(nEdge4);
            edges.Remove(edge);
            outerEdges.Remove(edge);
            if (nEdge3.isBorderEdge())
            {
                outerEdges.Add(nEdge3);
                outerEdges.Add(nEdge4);
            }
            if (isDeloneNeeded)
            {
                if (firstDelone) TestTriangulation(nTr1);
                if (secondDelone) TestTriangulation(nTr2);
            }
        }


















        public void DeleteTriangle(Triangle tr)
        {
            triangles.Remove(tr);

            foreach (TriangulationEdge edge in tr.edges)
            {
                edge.RemoveTriangle(tr);
                if (edge.triangles[0] == null && edge.triangles[1] == null)
                {
                    outerEdges.Remove(edge);
                    edges.Remove(edge);

                    //foreach (Point p in edge.GetPoints())
                    //{
                    //    bool isLost = true;
                    //    foreach (Triangle pTr in triangles)
                    //    {
                    //        if (pTr.IsHavePoint(p))
                    //        {
                    //            isLost = false;
                    //            break;
                    //        }
                    //    }
                    //    if (isLost)
                    //    {
                    //        GetPoints().Remove(p);
                    //    }
                    //}
                    continue;
                }
                if (edge.isBorderEdge() && !outerEdges.Contains(edge))
                {
                    outerEdges.Add(edge);
                }
            }
            for (int i = 0; i < planar.GetUpperBound(0); i++)
            {
                for (int j = 0; j < planar.GetUpperBound(1); j++)
                {
                    if (planar[i, j].GetTriangle() == tr)
                    {
                        foreach (Triangle bTr in tr.GetBorderTriangles())
                        {
                            if (bTr != null)
                            {
                                planar[i, j].SetTriangle(bTr);
                            }
                        }
                    }
                }
            }
        }

        public void DeleteEdge(TriangulationEdge edge)
        {
            if (edge.triangles[0] != null)
            {
                DeleteTriangle(edge.triangles[0]);
            }
            if (edge.triangles[1] != null)
            {
                DeleteTriangle(edge.triangles[1]);
            }
            if (edge.triangles[1] != null && edge.triangles[0] != null)
            {
                outerEdges.Remove(edge);
                edges.Remove(edge);
            }
        }

        /// <summary>
        /// Вычисляет выполняется ли условие Делоне для данных 2-х треугольников.
        /// </summary>
        /// <param name="tr1"> Первый треугольник. </param>
        /// <param name="tr2"> Второй треугольник. </param>
        /// <returns></returns>
        private bool IsDelone(Triangle tr1, Triangle tr2)
        {
            Point[] points = tr1.GetPoints(tr2);
            decimal dx0 = (points[0].X - points[1].X);
            decimal dx1 = (points[0].X - points[3].X);
            decimal dy0 = (points[0].Y - points[1].Y);
            decimal dy1 = (points[0].Y - points[3].Y);

            decimal dx2 = (points[2].X - points[1].X);
            decimal dx3 = (points[2].X - points[3].X);
            decimal dy2 = (points[2].Y - points[1].Y);
            decimal dy3 = (points[2].Y - points[3].Y);

            decimal st1 = dx0 * dx1 + dy0 * dy1;
            decimal st2 = dx2 * dx3 + dy2 * dy3;
            if (st1 >= 0m && st2 >= 0m)
            {
                return true;
            }

            return (Math.Abs(dx0 * dy1 - dx1 * dy0)
                * (st2)
                + (st1)
                * Math.Abs(dx3 * dy2 - dx2 * dy3)) >= 0m;
        }


        /// <summary>
        /// Вычисляет лежат ли точки на одной прямой
        /// </summary>
        /// <param name="p1"> Первая точка. </param>
        /// <param name="p2"> Вторая точка. </param>
        /// <param name="p3"> Третья точка. </param>
        /// <returns></returns>
        private bool IsCollinear(Point p1, Point p2, Point p3)
        {
            if (BMF.Deq((p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y), 0m))
            {
                return true;
            }
            return false;
        }


































































        /// <summary>
        /// Находит треугольник через простой итеративный алгоритм с динамическим массивом.
        /// </summary>
        /// <param name="point"> Искомая точка. </param>
        /// <returns></returns>
        public Triangle FindTriangle(Point point)
        {
            Random rand = new Random();
            //Triangle tr = triangles[rand.Next(triangles.Count - 1)];
            //MessageBox.Show(Math.Truncate((point.X - minX) / section).ToString() + "\n" + Math.Truncate((point.Y - minY) / section).ToString());
            Triangle tr = null;
            if (point.GetDistance2D(triangles.Last().GetCentroid()) < searchArray.GetSection())
            {
                tr = triangles.Last();
            }
            else
            {
                //tr = planar[Convert.ToInt32(Math.Truncate((point.X - minX) / section)), Convert.ToInt32(Math.Truncate((point.Y - minY) / section))].GetTriangle();
                tr = searchArray.GetTriangleByPoint(point);
            }
            Point Center;
            TriangulationEdge currentEdge = null;
            int count = 0;
            while (true)
            {
                //loopCount++;
                //if (loopCount == 10000000)
                //{
                //    MessageBox.Show("Залупливание в цикле поиска треугольника");
                //}

                Center = tr.GetCentroid();

                foreach (Point p in tr.GetPoints())
                {
                    if (p.IsEqual2D(point))
                    {
                        return tr;
                    }
                }

                foreach (TriangulationEdge edge in tr.edges)
                {
                    if (edge.isPointBelongToSegment(point))
                    {
                        return tr;
                    }
                    TriangulationEdge interEdge = new TriangulationEdge(Center, point);
                    Point pt = edge.GetIntersection2D(interEdge);
                    Point pInter = tr.GetPointBelongingToSegment(interEdge);
                    if ((pt == null || edge == currentEdge) && pInter == null)
                    {
                        count++;
                        continue;
                    }
                    if (pInter != null)
                    {
                        TriangulationEdge[] testEdges = tr.GetEdgesByPoint(tr.PointIntersect2D(pInter));
                        if (testEdges[0].isBorderEdge() && testEdges[1].isBorderEdge())
                        {
                            return null;
                        }
                        else if (testEdges[0].isBorderEdge())
                        {
                            tr = testEdges[1].GetOtherTriangle(tr);
                            currentEdge = testEdges[1];
                            break;
                        }
                        else
                        {
                            tr = testEdges[0].GetOtherTriangle(tr);
                            currentEdge = testEdges[0];
                            break;
                        }
                    }
                    tr = edge.GetOtherTriangle(tr);
                    currentEdge = edge;
                    break;
                }
                if (count >= 3 || tr == null)
                {
                    if (count > 3) MessageBox.Show("Что-то не так в поиске");
                    return tr;
                }
                count = 0;
            }
        }
        /// <summary>
        /// Ищет треугольник через итеративный алгоритм с динамическим массивом, при этом 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Triangle FindTriangle_Borders(Point point)
        {
            Random rand = new Random();
            Point testPoint = triangles.Last().GetCentroid();
            Triangle tr = null;
            if (point.GetDistance2D(testPoint) < searchArray.GetSection())
            {
                tr = triangles.Last();
            }
            else
            {
                //tr = planar[Convert.ToInt32(Math.Truncate((point.X - minX) / section)), Convert.ToInt32(Math.Truncate((point.Y - minY) / section))].GetTriangle();
                tr = searchArray.GetTriangleByPoint(point);
            }



            Point Center;
            TriangulationEdge currentEdge = null;
            int count = 0;
            while (true)
            {
                Center = tr.GetCentroid();
                foreach (Point p in tr.GetPoints())
                {
                    if (p.IsEqual2D(point))
                    {
                        return tr;
                    }
                }


                foreach (TriangulationEdge edge in tr.edges)
                {
                    if (edge.isBorderEdge()) count++;
                }
                if (count == 2)
                {
                    if (currentEdge != null)
                    {
                        foreach (TriangulationEdge edge in tr.edges)
                        {
                            if (!edge.isBorderEdge())
                            {
                                currentEdge = edge;
                                tr = edge.GetOtherTriangle(tr);
                            }
                        }
                        continue;
                    }
                    return FindTriangle_Brute(point);
                }
                else if (count > 2)
                {
                    throw new AggregateException("Обнаружен треугольник вне триангуляции");
                }
                count = 0;
                foreach (TriangulationEdge edge in tr.edges)
                {
                    Point pt = edge.GetIntersection2D(Center, point);
                    if (edge.isPointBelongToSegment(point))
                    {
                        return tr;
                    }
                    if (pt == null || edge == currentEdge)
                    {
                        count++;
                        continue;
                    }
                    if (tr.IsPointIntersect2D(pt))
                    {
                        TriangulationEdge[] testEdges = tr.GetEdgesByPoint(tr.PointIntersect2D(pt));
                        if (testEdges[0].isBorderEdge() && testEdges[1].isBorderEdge())
                        {
                            return null;
                        }
                        else if (testEdges[0].isBorderEdge())
                        {
                            tr = testEdges[1].GetOtherTriangle(tr);
                            currentEdge = testEdges[1];
                            break;
                        }
                        else
                        {
                            tr = testEdges[0].GetOtherTriangle(tr);
                            currentEdge = testEdges[0];
                            break;
                        }
                    }
                    if (edge.isBorderEdge())
                    {
                        if (currentEdge == null)
                        {
                            currentEdge = tr.edges[0] == edge ? tr.edges[1] : tr.edges[0];
                            tr = currentEdge.GetOtherTriangle(tr);
                            break;
                        }
                        currentEdge = tr.GetOtherEdge(currentEdge, edge);
                        tr = currentEdge.GetOtherTriangle(tr);
                        break;
                    }
                    tr = edge.GetOtherTriangle(tr);
                    currentEdge = edge;
                    break;
                }
                if (count >= 3 || tr == null)
                {
                    return tr;
                }
                count = 0;
            }
        }

        public Triangle FindTriangle_Brute(Point point)
        {
            foreach (Triangle tr in triangles)
            {
                if (tr.IsInside(point)) return tr;
            }
            return null;
        }






























































        // Перебрасывает ребро двух соседних треугольников
        public void FlipEdge(Triangle tr1, Triangle tr2, bool isDelone = true, bool isPlanar = true)
        {
            if (!tr1.IsHaveCommonEdge(tr2))
            {
                MessageBox.Show("Треугольники без общей грани");
                return;
            }
            if (tr1 == tr2)
            {
                return;
            }
            TriangulationEdge commonEdge = tr1.GetCommonEdge(tr2);
            Point[] points1 = tr1.GetPoints();
            Point[] points2 = tr2.GetPoints();
            Point p1 = null;
            Point p2 = null;
            for (int i = 0; i < 3; i++)
            {
                if (!commonEdge.IsContainingPoint(points1[i]))
                {
                    p1 = points1[i];
                }
                if (!commonEdge.IsContainingPoint(points2[i]))
                {
                    p2 = points2[i];
                }
            }
            try
            {
                if (p1 == p2)
                {
                    Point[] poin = tr1.GetPoints();
                    MessageBox.Show($"Строка проверки треугольников:\nРавны ли треугольники: {tr1 == tr2}\nРавны ли их рёбра 1: {tr1.GetEdgeByPoints(poin[0], poin[1]) == tr2.GetEdgeByPoints(poin[0], poin[1])}\n" +
                        $"Равны ли их рёбра 2: {tr1.GetEdgeByPoints(poin[1], poin[2]) == tr2.GetEdgeByPoints(poin[1], poin[2])}\n" +
                        $"Равны ли их рёбра 3: {tr1.GetEdgeByPoints(poin[2], poin[0]) == tr2.GetEdgeByPoints(poin[2], poin[0])}\n");
                    //throw new AggregateException();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка при переброске ребра{tr1}\n\n{tr2}");
            }
            Point center = new TriangulationEdge(p1, p2).GetMiddlePoint();
            if (!tr1.IsInside(center) && !tr2.IsInside(center))
            {
                return;
            }
            TriangulationEdge tempEdge1Tr1 = tr1.GetEdgeByPoints(commonEdge.points[0], p1);
            TriangulationEdge tempEdge2Tr1 = tr1.GetEdgeByPoints(commonEdge.points[1], p1);
            TriangulationEdge tempEdge1Tr2 = tr2.GetEdgeByPoints(commonEdge.points[0], p2);
            TriangulationEdge tempEdge2Tr2 = tr2.GetEdgeByPoints(commonEdge.points[1], p2);
            commonEdge.PseudoConstruct(p1, p2);
            tr1.PseudoConstruct(commonEdge, tempEdge1Tr1, tempEdge1Tr2, tr2, tr2, true);
            tr2.PseudoConstruct(commonEdge, tempEdge2Tr1, tempEdge2Tr2, tr1, tr1, true);
            if (isPlanar)
            {
                //planar[Convert.ToInt32(Math.Truncate((tr1.GetCentroid().X - minX) / section)), Convert.ToInt32(Math.Truncate((tr1.GetCentroid().Y - minY) / section))].SetTriangle(tr1);
                //planar[Convert.ToInt32(Math.Truncate((tr2.GetCentroid().X - minX) / section)), Convert.ToInt32(Math.Truncate((tr2.GetCentroid().Y - minY) / section))].SetTriangle(tr2);

                searchArray.AddTriangle(tr1);
                searchArray.AddTriangle(tr2);
            }
            if (isDelone)
            {
                TestTriangulation(tr1);
                TestTriangulation(tr2);
            }

            return;
        }
        public void FlipEdge(TriangulationEdge edge, bool isDelone = true, bool isPlanar = true)
        {
            if (edge.isBorderEdge())
            {
                return;
            }
            Triangle tr1 = edge.triangles[0];
            Triangle tr2 = edge.triangles[1];
            FlipEdge(tr1, tr2, isDelone, isPlanar);
        }

        public void TestTriangulation(Triangle triangle)
        {
            if (triangle == null)
            {
                return;
            }
            Triangle[] testTriangles = triangle.GetBorderTriangles();
            foreach (Triangle tr in testTriangles)
            {
                if (tr == null)
                {
                    continue;
                }




                //if (tr.IsEqual2D(triangle))
                //{
                //    MessageBox.Show($"Обнаружены равные треугольники при проверке условия делоне{tr}\n\n{triangle}");
                //}



                //MessageBox.Show(tr.ToString() + "\n\n" + triangle.ToString());
                if (!IsDelone(triangle, tr))
                {
                    FlipEdge(triangle, tr);
                    break;
                }
            }
            return;
        }









































        public void CreateIsolines(decimal step)
        {
            try
            {
                innerIsolines.Clear();
                outerIsolines.Clear();

                Point min = searchArray.GetMin();
                Point max = searchArray.GetMax();

                decimal absoluteHeight = min.Z + (step - (min.Z % step));
                decimal currentHeight = absoluteHeight;
                bool isEdgeOnPlaneHeight = false;
                while (absoluteHeight < max.Z)
                {
                    SetAllTrianglesStatus(0);
                    MarkTrianglesForIsoline(currentHeight);
                    foreach (Triangle tr in triangles)
                    {
                        if (tr.GetStatus() == 0) continue;
                        if (tr.IsEdgeOnHeightPlane(currentHeight))
                        {
                            isEdgeOnPlaneHeight = true;
                            break;
                        }
                    }
                    if (isEdgeOnPlaneHeight)
                    {
                        isEdgeOnPlaneHeight = false;
                        currentHeight += step / 100;
                        continue;
                    }
                    CreateBorderIsoline(currentHeight);
                    CreateInnerIsoline(currentHeight);
                    absoluteHeight += step;
                    currentHeight = absoluteHeight;
                }
                //MessageBox.Show(maxZ.ToString() + "\n" + minZ.ToString());

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        public void CreateBorderIsoline(decimal height)
        {
            foreach (TriangulationEdge edge in outerEdges)
            {
                Triangle startTriangle = null;
                LinkedList<Point> isoline = null;
                if (IsHeightInEdge(edge, height) && edge.GetOtherTriangle(null).GetStatus() == 1)
                {
                    startTriangle = edge.GetOtherTriangle(null);
                }
                if (startTriangle != null)
                {
                    isoline = new LinkedList<Point>();
                    Triangle currentTriangle = startTriangle;
                    TriangulationEdge currentEdge = edge;
                    isoline.AddLast(BMF.InterpolToPoint(edge, height));
                    bool isLastPointFound = false;


                    foreach (TriangulationEdge trEdge in currentTriangle.edges)
                    {
                        if (trEdge == currentEdge) continue;
                        if (currentTriangle.GetStatus() == 0)
                        {
                            isLastPointFound = true;
                            break;
                        }
                        if (IsHeightInEdge(trEdge, height))
                        {
                            isoline.AddLast(BMF.InterpolToPoint(trEdge, height));
                            currentTriangle.SetStatus(0);
                            currentEdge = trEdge;
                            currentTriangle = trEdge.GetOtherTriangle(currentTriangle);
                            if (trEdge.isBorderEdge())
                            {
                                isLastPointFound = true;
                            }
                            break;
                        }
                    }

                    if (!isLastPointFound)
                    {
                        while (true)
                        {
                            foreach (TriangulationEdge trEdge in currentTriangle.edges)
                            {
                                if (trEdge == currentEdge) continue;
                                if (currentTriangle.GetStatus() == 0)
                                {
                                    isLastPointFound = true;
                                    break;
                                }
                                if (IsHeightInEdge(trEdge, height) && !isLastPointFound)
                                {
                                    isoline.AddLast(BMF.InterpolToPoint(trEdge, height));
                                    currentTriangle.SetStatus(0);
                                    currentTriangle = trEdge.GetOtherTriangle(currentTriangle);
                                    currentEdge = trEdge;
                                    if (trEdge.isBorderEdge())
                                    {
                                        isLastPointFound = true;
                                    }
                                    break;
                                }
                            }
                            if (isLastPointFound)
                            {
                                break;
                            }
                        }
                    }
                }
                outerIsolines.Add(isoline);
            }
        }





        public void CreateInnerIsoline(decimal height)
        {
            foreach (Triangle tr in triangles)
            {
                if (tr.GetStatus() == 0) continue;
                LinkedList<Point> isoline = new LinkedList<Point>();
                Triangle currentTriangle = tr;
                TriangulationEdge currentEdge = null;
                TriangulationEdge firstEdge = null;
                bool isLastPointFound = false;
                foreach (TriangulationEdge trEdge in currentTriangle.edges)
                {
                    if (IsHeightInEdge(trEdge, height))
                    {
                        isoline.AddLast(BMF.InterpolToPoint(trEdge, height));
                        currentTriangle.SetStatus(0);
                        currentEdge = trEdge;
                        firstEdge = trEdge;
                        currentTriangle = trEdge.GetOtherTriangle(currentTriangle);
                        break;
                    }
                }
                while (true)
                {
                    foreach (TriangulationEdge trEdge in currentTriangle.edges)
                    {
                        if (trEdge == currentEdge) continue;
                        if (currentTriangle.GetStatus() == 0)
                        {
                            isLastPointFound = true;
                            break;
                        }
                        if (IsHeightInEdge(trEdge, height) && !isLastPointFound)
                        {
                            isoline.AddLast(BMF.InterpolToPoint(trEdge, height));
                            currentTriangle.SetStatus(0);
                            currentEdge = trEdge;
                            currentTriangle = trEdge.GetOtherTriangle(currentTriangle);
                            break;
                        }
                    }
                    if (isLastPointFound)
                    {
                        break;
                    }
                }
                innerIsolines.Add(isoline);
            }
        }
        private void MarkTrianglesForIsoline(decimal height)
        {
            foreach (Triangle tr in triangles)
            {
                foreach (TriangulationEdge edge in tr.edges)
                {
                    if (IsHeightInEdge(edge, height))
                    {
                        tr.SetStatus(1);
                        break;
                    }
                }
            }
        }
        private bool IsHeightInEdge(TriangulationEdge edge, decimal height)
        {
            if ((edge.points[0].Z <= height && height <= edge.points[1].Z) || (edge.points[0].Z >= height && height >= edge.points[1].Z))
            {
                return true;
            }
            return false;
        }
        private void SetAllTrianglesStatus(short status)
        {
            foreach (Triangle tr in triangles)
            {
                tr.SetStatus(status);
            }
        }

















































        public TriangulationEdge GetEdgeByPoints(Point p1, Point p2)
        {
            //Point midPoint = new Point(p1.X + (p2.X - p1.X) / 2, p1.Y + (p2.Y - p1.Y) / 2, p1.Z + (p2.Z - p1.Z)/2);
            //Triangle tr = FindTriangle(midPoint);
            foreach (TriangulationEdge edge in edges)
            {
                if (edge.IsEqual2D(p1, p2)) return edge;
            }
            return null;
        }




        public Point GetPointById(string id)
        {
            foreach (Point point in points)
            {
                if (point.id.Equals(id))
                {
                    return point;
                }
            }
            return null;
        }
        public TriangulationEdge GetEdgeById(string id)
        {
            TriangulationEdge result = null;
            foreach (TriangulationEdge edge in edges)
            {
                if (edge.id.Equals(id))
                {
                    result = edge;
                    return result;
                }
            }
            return result;
        }
        public Triangle GetTriangleById(string id)
        {
            Triangle result = null;
            foreach (Triangle tr in triangles)
            {
                if (tr.id.Equals(id))
                {
                    result = tr;
                    return result;
                }
            }
            return result;
        }
        public List<Triangle> GetTrianglesByPoint(Point point)
        {
            List<Triangle> result = new List<Triangle>();
            foreach (Triangle tr in triangles)
            {
                if (tr.IsHavePoint(point))
                {
                    result.Add(tr);
                }
            }
            return result;
        }
        public List<TriangulationEdge> GetEdgesByPoint(Point point, bool isOnlyBorder = false)
        {
            List<TriangulationEdge> result = new List<TriangulationEdge>();
            foreach (TriangulationEdge edge in edges)
            {
                if (edge.IsContainingPoint(point))
                {
                    if (isOnlyBorder && edge.isBorderEdge())
                    {
                        result.Add(edge);
                    }
                    if (!isOnlyBorder)
                    {
                        result.Add(edge);
                    }
                }
            }
            return result;
        }

        public Point GetFullIntersection(TriangulationEdge edge, bool CheckForBorderPoints = true)
        {
            Point result = null;
            foreach (TriangulationEdge tEdge in edges)
            {
                result = tEdge.GetIntersection2D(edge, CheckForBorderPoints);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        public bool IsIntersect2D(Point p)
        {
            foreach (Point point in points)
            {
                if (point.IsEqual2D(p))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsEdgeIntersectWithTriangulation(TriangulationEdge edge, bool isPointsConsidered = true, TriangulationEdge[] excludedEdges = null)
        {
            foreach (TriangulationEdge tEdge in edges)
            {
                if (excludedEdges != null && excludedEdges.Contains(tEdge))
                {
                    continue;
                }
                Point intersection = tEdge.GetIntersection2D(edge);
                if (intersection != null)
                {
                    if ((intersection.IsEqual2D(edge.points[0]) || intersection.IsEqual2D(edge.points[1]) || intersection.IsEqual2D(tEdge.points[0]) || intersection.IsEqual2D(tEdge.points[1])) && !isPointsConsidered)
                    {
                        continue;
                    }
                    return true;
                }
            }
            return false;
        }















































        //private const string saveString1 = "VERTEX";
        //private const string saveString2 = "SKIP";
        //private const string saveString3 = "SKIPV";
        //private const string saveString4 = "VERTEX0";
        //private const string saveString5 = "VERTEX1";



        private const string saveString1 = "V";
        private const string saveString2 = "S";
        private const string saveString3 = "SV";
        private const string saveString4 = "V0";
        private const string saveString5 = "V1";

        /// <summary>
        /// Сохраняет триангуляцию в файл по заданному пути. Если файла по пути нет, то он создаётся.
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        public void Save(string filePath)
        {



            Queue<TriangulationEdge> ActiveEdges = new Queue<TriangulationEdge>();
            Triangle StartTriangle = this.triangles[0];
            if (StartTriangle == null) return;

            StringBuilder saveString = new StringBuilder();
            StartTriangle.SetStatus(2);

            Point[] tPoints = StartTriangle.GetPoints();
            //tPoints[0].id = "1";
            //tPoints[1].id = "2";
            //tPoints[2].id = "3";

            ActiveEdges.Enqueue(StartTriangle.GetEdgeByPoints(tPoints[0], tPoints[1]));
            ActiveEdges.Enqueue(StartTriangle.GetEdgeByPoints(tPoints[1], tPoints[2]));
            ActiveEdges.Enqueue(StartTriangle.GetEdgeByPoints(tPoints[2], tPoints[0]));

            StartTriangle.edges[0].SetStatus(2);
            StartTriangle.edges[1].SetStatus(2);
            StartTriangle.edges[2].SetStatus(2);

            saveString.Append(tPoints[0]);
            saveString.Append(tPoints[1]);
            saveString.Append(tPoints[2]);
            saveString.Append(StartTriangle.GetEdgeByPoints(tPoints[1], tPoints[2]).points[0] == tPoints[1] ? "0\n" : "1\n");
            saveString.Append(StartTriangle.GetEdgeByPoints(tPoints[2], tPoints[0]).points[0] == tPoints[2] ? "0\n" : "1\n");
            tPoints[0].SetStatus(2);
            tPoints[1].SetStatus(2);
            tPoints[2].SetStatus(2);


            for (; ActiveEdges.Count > 0;)
            {
                TriangulationEdge edge = ActiveEdges.Dequeue();
                if (edge.isBorderEdge())
                {
                    saveString.Append(saveString2 + "\n");
                    continue;
                }
                Triangle currentTriangle = edge.triangles[0].GetStatus() == 2 ? edge.triangles[1] : edge.triangles[0];
                int count = 0;
                foreach (TriangulationEdge currentTriangleEdge in currentTriangle.edges)
                {
                    if (currentTriangleEdge.GetStatus() == 2) count++;
                }
                switch (count)
                {
                    case 0:
                        throw new AggregateException("Во время записи выбран \"чистый\" треугольник");
                    case 1:
                        Point point = currentTriangle.GetOtherPoint(edge.points[0], edge.points[1]);
                        if (point == null) throw new AggregateException("Что-то не так со ссылками на треугольники в ребре");
                        if (point.GetStatus() == 2)
                        {
                            saveString.Append(saveString3 + "\n");
                        }
                        else
                        {
                            
                            saveString.Append($"{saveString1}\n{point}");
                            TriangulationEdge[] edges = currentTriangle.GetEdgesByPoint(point);
                            saveString.Append(point == currentTriangle.GetEdgeByPoints(point, edge.points[0]).points[0] ? "0\n" : "1\n");
                            saveString.Append(point == currentTriangle.GetEdgeByPoints(point, edge.points[1]).points[0] ? "0\n" : "1\n");
                            ActiveEdges.Enqueue(currentTriangle.GetEdgeByPoints(point, edge.points[0]));
                            ActiveEdges.Enqueue(currentTriangle.GetEdgeByPoints(point, edge.points[1]));
                            currentTriangle.SetStatus(2);
                            edges[0].SetStatus(2);
                            edges[1].SetStatus(2);
                            point.SetStatus(2);
                        }
                        break;
                    case 2:
                        Point point1 = currentTriangle.GetOtherPoint(edge.points[0], edge.points[1]);
                        if (point1 == null) throw new AggregateException("Что-то не так со ссылками на треугольники в ребре");
                        if (point1.GetStatus() != 2)
                        {
                            throw new AggregateException("Статус точки не был изменён не смотря на использование");
                        }
                        else
                        {
                            //Vertex1
                            if (currentTriangle.GetEdgeByPoints(point1, edge.points[0]).GetStatus() == 2)
                            {
                                saveString.Append($"{saveString5}\n");
                                TriangulationEdge fEdge = currentTriangle.GetEdgeByPoints(point1, edge.points[1]);
                                ActiveEdges.Enqueue(fEdge);
                                currentTriangle.SetStatus(2);
                                fEdge.SetStatus(2);
                                saveString.Append(point1 == fEdge.points[0] ? "0\n" : "1\n");
                            }
                            //Vertex0
                            else if (currentTriangle.GetEdgeByPoints(point1, edge.points[1]).GetStatus() == 2)
                            {
                                saveString.Append($"{saveString4}\n");
                                TriangulationEdge fEdge = currentTriangle.GetEdgeByPoints(point1, edge.points[0]);
                                ActiveEdges.Enqueue(fEdge);
                                currentTriangle.SetStatus(2);
                                fEdge.SetStatus(2);
                                saveString.Append(point1 == fEdge.points[0] ? "0\n" : "1\n");
                            }
                        }
                        break;
                    case 3:
                        //throw new AggregateException("Во время записи выбран уже записанный треугольник");
                        saveString.Append(saveString3 + "\n");
                        currentTriangle.SetStatus(2);
                        break;
                    default:
                        throw new AggregateException("Во время записи Слишком большое значение в count");
                }
            }
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false, System.Text.Encoding.Default))
                {
                    writer.WriteLine(saveString);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }





















        /// <summary>
        /// Загружает триангуляцию из файла. Если файл не существует - выбрасывается ошибка.
        /// </summary>
        /// <param name="filePath">Путь к загружаемому файлу</param>
        public void Load(string filePath)
        {
            this.clear();
            //try { 
            using (StreamReader reader = new StreamReader(filePath, System.Text.Encoding.Default))
            {

                Queue<TriangulationEdge> ActiveEdges = new Queue<TriangulationEdge>();
                string line = reader.ReadLine();
                Point p1 = new Point(line);
                line = reader.ReadLine();
                Point p2 = new Point(line);
                line = reader.ReadLine();
                Point p3 = new Point(line);
                TriangulationEdge edge1 = new TriangulationEdge(p1, p2);
                line = reader.ReadLine();
                TriangulationEdge edge2 = null;
                if (line.Equals("0"))
                {
                    edge2 = new TriangulationEdge(p2, p3);
                }
                else
                {
                    edge2 = new TriangulationEdge(p3, p2);
                }
                line = reader.ReadLine();
                TriangulationEdge edge3 = null;
                if (line.Equals("0"))
                {
                    edge3 = new TriangulationEdge(p3, p1);
                }
                else
                {
                    edge3 = new TriangulationEdge(p1, p3);
                }
                Triangle triangle = new Triangle(edge1, edge2, edge3);
                this.triangles.Add(triangle);
                this.edges.Add(edge1);
                this.edges.Add(edge2);
                this.edges.Add(edge3);
                ActiveEdges.Enqueue(edge1);
                ActiveEdges.Enqueue(edge2);
                ActiveEdges.Enqueue(edge3);
                this.points.Add(p1);
                this.points.Add(p2);
                this.points.Add(p3);
                for (; ActiveEdges.Count > 0;)
                {
                    line = reader.ReadLine();
                    TriangulationEdge edge = ActiveEdges.Dequeue();
                    switch (line)
                    {
                        //VERTEX
                        case saveString1:
                            line = reader.ReadLine();
                            Point Npoint = new Point(line);
                            line = reader.ReadLine();
                            TriangulationEdge nEdge1 = null;
                            if (line.Equals("0"))
                            {
                                nEdge1 = new TriangulationEdge(Npoint, edge.points[0]);
                            }
                            else
                            {
                                nEdge1 = new TriangulationEdge(edge.points[0], Npoint);
                            }
                            line = reader.ReadLine();
                            TriangulationEdge nEdge2 = null;
                            if (line.Equals("0"))
                            {
                                nEdge2 = new TriangulationEdge(Npoint, edge.points[1]);
                            }
                            else
                            {
                                nEdge2 = new TriangulationEdge(edge.points[1], Npoint);
                            }
                            Triangle nTriangle = new Triangle(edge, nEdge1, nEdge2);
                            ActiveEdges.Enqueue(nEdge1);
                            ActiveEdges.Enqueue(nEdge2);
                            this.triangles.Add(nTriangle);
                            this.edges.Add(nEdge1);
                            this.edges.Add(nEdge2);
                            this.points.Add(Npoint);
                            break;
                        //SKIP
                        case saveString2:
                            this.outerEdges.Add(edge);
                            break;
                        //SKIPV
                        case saveString3:
                            break;
                        //VERTEX0
                        case saveString4:
                            line = reader.ReadLine();
                            Point loosePoint = edge.points[1];
                            List<TriangulationEdge> pointEdges = this.GetEdgesByPoint(loosePoint, true);
                            TriangulationEdge secondEdge = pointEdges[pointEdges[0] == edge ? 1 : 0];
                            Point targetPoint = secondEdge.GetOtherPoint(loosePoint);
                            TriangulationEdge nEdge = null;
                            if (line.Equals("0"))
                            {
                                nEdge = new TriangulationEdge(targetPoint, edge.points[0]);
                            }
                            else
                            {
                                nEdge = new TriangulationEdge(edge.points[0], targetPoint);
                            }
                            triangles.Add(new Triangle(edge, secondEdge, nEdge));
                            ActiveEdges.Enqueue(nEdge);
                            edges.Add(nEdge);
                            break;
                        //VERTEX1
                        case saveString5:
                            line = reader.ReadLine();
                            Point loosePoint1 = edge.points[0];
                            List<TriangulationEdge> pointEdges1 = this.GetEdgesByPoint(loosePoint1, true);
                            TriangulationEdge secondEdge1 = pointEdges1[pointEdges1[0] == edge ? 1 : 0];
                            Point targetPoint1 = secondEdge1.GetOtherPoint(loosePoint1);
                            TriangulationEdge nEdge3 = null;
                            if (line.Equals("0"))
                            {
                                nEdge3 = new TriangulationEdge(targetPoint1, edge.points[1]);
                            }
                            else
                            {
                                nEdge3 = new TriangulationEdge(edge.points[1], targetPoint1);
                            }
                            triangles.Add(new Triangle(edge, secondEdge1, nEdge3));
                            ActiveEdges.Enqueue(nEdge3);
                            edges.Add(nEdge3);
                            break;
                    }
                }
            }
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.Message);
            //}

        }
        private void clear()
        {
            loopCount = 0;
            precision = 5;

            //currentPlanarMod = 1;
            //planarMod = 3;
            //planarBorder = 4 * planarMod;
            //section = 0;
            planar = new LinkCell[1,1];

            points = new List<Point>();
            edges = new List<TriangulationEdge>();
            triangles = new List<Triangle>();
            outerEdges = new List<TriangulationEdge>();
            outerIsolines = new List<LinkedList<Point>>();
            innerIsolines = new List<LinkedList<Point>>();

            searchArray = null;

            SetStatus(0);
        }

    }
}
