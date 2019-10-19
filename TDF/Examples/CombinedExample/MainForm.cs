using System;
using System.Windows.Forms;
using Engine.Graphics.GUI;
using SharpDX;
using TDF.Core;
using TDF.Graphics.Cameras;
using TDF.Graphics.Data;
using TDF.Graphics.Models;
using TDF.Graphics.Render;
using TDF.Inputs;

namespace CombinedExample
{
    public partial class MainForm : Form
    {
        private readonly FreeCamera _freeCamera; // камера
        private readonly Box _box = new Box(); // цветной 2D квадрат
        private readonly Bitmap _bit = new Bitmap(); // 2D затекстурированный квадрат
        private readonly DxModel _plos = new DxModel(); // 3D модель (тоже квадрат)

        private double i; // градус поворота
        public MainForm()
        {
            InitializeComponent();
            _freeCamera = new FreeCamera(new Vector3(0, 25, -150), 0, 0, true); // созадем камеру
            DirectX11.Initialize(directXPanel1.Handle, _freeCamera, "config.ini", true); // инициализируем ДиректИкс

            Height = Config.Height;
            Width = Config.Width;

            //создаем 3D модель
            _plos = GeometryGenerator.GenereateModel<TextureVertex>(GeometryGenerator.CreateGrid(1000, 1000, 3, 3));

            _box.Initialize(new Size2(200,200));// Создаем квадраты
            _bit.Initialize(new Point(20,20));

            Shown += (sender, args) => directXPanel1.Run(Update, Draw); // запускаем конвеер!
        }
        public void Draw()
        {
            DirectX11.TurnZBufferOn(); // включаем 3D режим (на самом деле включаем буфер глубины)
            _plos.Render(); // рисуем модель
            DirectX11.TurnZBufferOff(); // выключаем
            _box.Render((int)(50 * Math.Cos(i+180) + 100), (int)(50 * Math.Sin(i+180) + 100)); //крутим
            _bit.Render((int)(100 * Math.Cos(i) + Config.Width / 2 - 100), (int)(100 * Math.Sin(i) + Config.Height / 2 - 100));
            label1.Text = directXPanel1.FPS.ToString(); // пишем ФПС
        }

        public new void Update()
        {
            i += 0.1;
            Input.SetMouseCoord(WinAPI.GetCurPos()); // получаем координты

            _freeCamera.RotateWithMouse(); // крутим камеру мышкой

            if (Input.IsKeyDown(Key.W)) //ловим нажатие и двигаемся
                _freeCamera.DirectionMove(Vector3.ForwardLH);
            else if (Input.IsKeyDown(Key.S))
                _freeCamera.DirectionMove(Vector3.BackwardLH);
            else if (Input.IsKeyDown(Key.A))
                _freeCamera.DirectionMove(Vector3.Left);
            else if (Input.IsKeyDown(Key.D))
                _freeCamera.DirectionMove(Vector3.Right);
            _bit.SetSize(new Size2(100,(int)(20*i)));
            _freeCamera.Update(); // обновляем камеру
        }
    }
}
