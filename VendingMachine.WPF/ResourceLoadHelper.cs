using System.IO;
using System.Threading;
using System.Collections.Generic;
using VendingMachineDataInjectionInterfaces;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using VendingMachine.WPF.Properties;
using System;

namespace VendingMachine.WPF
{
    [Obsolete("System.Drawing usage")]
    public abstract class BitmapShape
    {
        [Obsolete("System.Drawing usage")]
        public abstract System.Drawing.Bitmap Render(System.Drawing.Graphics g = null, System.Drawing.Bitmap b = null, params int[] options);

        [Obsolete("System.Drawing usage")]
        public virtual System.Drawing.Bitmap Render(Size size)
        {
            return new System.Drawing.Bitmap((int)size.Width + 1, (int)size.Height + 1);
        }

        public virtual void Initialize(params int[] options)
        {
        }

        public virtual Image GetImage(object bmpCanvas = null)
        {
            return null;
        }

        public virtual Cursor GetCursor()
        {
            return null;
        }

        public virtual Size Size { get; set; }

        private float _dpi;
        [Obsolete("System.Drawing usage")]
        public virtual System.Drawing.Size InnerSize
        {
            get
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
                {
                    _dpi = g.DpiX;
                    return new System.Drawing.Size { Width = (int)(Size.Width * g.DpiX / 96), Height = (int)(Size.Height * g.DpiY / 96) };
                }
            }
        }

        public bool IsHighDPI
        {
            get { return _dpi > 100; }
        }

        public virtual Color BackColor { get; set; }

        [Obsolete("System.Drawing usage")]
        public virtual System.Drawing.Color DrawingBackColor
        {
            get { return Convert(BackColor); }
        }

        public virtual Color ForeColor { get; set; }

        [Obsolete("System.Drawing usage")]
        public virtual System.Drawing.Color DrawingForeColor
        {
            get { return Convert(ForeColor); }
        }

        [Obsolete("System.Drawing usage")]
        public static System.Drawing.Color Convert(Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }

    [Obsolete("System.Drawing usage")]
    public class BaseShapeDecorator : BitmapShape
    {
        private bool _szChanged = false;
        private System.Drawing.Size _innerSize;
        protected readonly BitmapShape _shape;
        private int[] _options;

        [Obsolete("System.Drawing usage")]
        public BaseShapeDecorator(BitmapShape shape, params int[] options)
        {
            _innerSize = new System.Drawing.Size(options.Length > 0 ? options[0] : 32, 32);
            _shape = shape;
            _options = options;
        }

        public override void Initialize(params int[] options)
        {
            _options = options;
        }

        public override Size Size
        {
            get { return _shape.Size; }
            set
            {
                if (_szChanged = _shape.Size.Height != value.Height || _shape.Size.Width != value.Width)
                {
                    _shape.Size = value;
                }
            }
        }

        [Obsolete("System.Drawing usage")]
        public override System.Drawing.Size InnerSize
        {
            get
            {
                if (_szChanged)
                {
                    _innerSize = _shape.InnerSize;
                }
                return _innerSize;
            }
        }

        public override Color BackColor
        {
            get { return _shape.BackColor; }
            set { _shape.BackColor = value; }
        }

        public override Color ForeColor
        {
            get { return _shape.ForeColor; }
            set { _shape.ForeColor = value; }
        }

        [Obsolete("System.Drawing usage")]
        public override System.Drawing.Bitmap Render(System.Drawing.Graphics g = null, System.Drawing.Bitmap b = null, params int[] options)
        {
            return new System.Drawing.Bitmap(InnerSize.Width, InnerSize.Height);
        }

        [Obsolete("System.Drawing usage")]
        public override Image GetImage(object bmpCanvas = null)
        {
            System.Drawing.Bitmap b = bmpCanvas as System.Drawing.Bitmap ?? Render();
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b))
            {
                return Convert(_shape.Render(g, b, _options), true);
            }
        }

        [Obsolete("System.Drawing usage")]
        public override Cursor GetCursor()
        {
            System.Drawing.Bitmap b = Render();
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b))
            {
                return Convert(_shape.Render(g, b, _options));
            }
        }

        [Obsolete("System.Drawing usage")]
        public static Color Convert(System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        [Obsolete("System.Drawing usage")]
        public static Image Convert(System.Drawing.Bitmap bmp, bool dispose)
        {
            BitmapImage bmpExport = new BitmapImage();
            using (MemoryStream mem = new MemoryStream())
            {
                bmpExport.BeginInit();
                bmpExport.CacheOption = BitmapCacheOption.OnLoad;
                bmpExport.SourceRect = new Int32Rect(0, 0, bmp.Width, bmp.Height);
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                bmpExport.StreamSource = mem;
                bmpExport.EndInit();
            }
            if (dispose)
            {
                bmp.Dispose();
            }
            Image img = new Image();
            img.Source = bmpExport;
            img.Width = bmpExport.Width;
            img.Height = bmpExport.Height;
            return img;
        }

        [Obsolete("System.Drawing usage")]
        public static Cursor Convert(System.Drawing.Bitmap bmp)
        {
            Cursor cursor;
            using (MemoryStream mem = new MemoryStream())
            {
                System.Drawing.Imaging.ImageCodecInfo[] encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                byte[] pngData = mem.ToArray();
                int size = pngData.GetLength(0);
                using (MemoryStream mm = new MemoryStream())
                {
                    mm.Write(BitConverter.GetBytes((ushort)0), 0, 2);
                    mm.Write(BitConverter.GetBytes((ushort)2), 0, 2);
                    mm.Write(BitConverter.GetBytes((ushort)1), 0, 2);
                    mm.WriteByte(32);
                    mm.WriteByte(32);
                    mm.WriteByte(0);
                    mm.WriteByte(0);
                    mm.Write(BitConverter.GetBytes((ushort)15), 0, 2);
                    mm.Write(BitConverter.GetBytes((ushort)15), 0, 2);
                    mm.Write(BitConverter.GetBytes(size), 0, 4);
                    mm.Write(BitConverter.GetBytes((uint)22), 0, 4);
                    mm.Write(pngData, 0, size);
                    mm.Seek(0, SeekOrigin.Begin);
                    cursor = new Cursor(mm);
                }
                bmp.Dispose();
            }
            return cursor;
        }
    }

    [Obsolete("System.Drawing usage")]
    public class CoinsImage : BitmapShape
    {
        public override System.Drawing.Bitmap Render(System.Drawing.Graphics g, System.Drawing.Bitmap b, params int[] options)
        {
            System.Drawing.Color bc = DrawingBackColor;
            System.Drawing.Color fc = DrawingForeColor;
            System.Drawing.Rectangle r = System.Drawing.Rectangle.Empty;
            System.Drawing.Size o = InnerSize;
            bool vm = (AccountType)options[0] == AccountType.VendingMachine;
            System.Drawing.Size sz = vm ? new System.Drawing.Size(o.Width - 1, o.Width * 2 / 3) : new System.Drawing.Size(o.Height * 2 / 3, o.Height - 1);
            int w = (vm ? (o.Height - o.Width / 3) : (o.Width - o.Height / 3)) / options[3];
            for (int i = 0; i < (options[2] > options[3] ? options[3] : options[2]); i++)
            {
                r = vm ? new System.Drawing.Rectangle(0, i * w, sz.Width, sz.Height) : new System.Drawing.Rectangle(i * w, 0, sz.Width, sz.Height);
                g.FillEllipse(new System.Drawing.SolidBrush(bc), r);
                g.DrawEllipse(new System.Drawing.Pen(new System.Drawing.SolidBrush(fc)), r);
            }
            if (!r.IsEmpty)
            {
                string s = options[1].ToString();
                if (vm)
                {
                    if (IsHighDPI)
                    {
                        r.Inflate(s.Length > 1 ? -2 : -9, -1);
                    }
                    else
                    {
                        r.Inflate(s.Length > 1 ? -1 : -7, -1);
                    }
                }
                else
                {
                    if (IsHighDPI)
                    {
                        r.Inflate(s.Length > 1 ? 4 : -4, -8);
                    }
                    else
                    {
                        r.Inflate(s.Length > 1 ? 4 : -3, -5);
                    }
                }
                System.Drawing.Font fnt = new System.Drawing.Font("Microsoft Sans Serif", 12, System.Drawing.FontStyle.Bold);
                g.DrawString(s, fnt, new System.Drawing.SolidBrush(fc), r);
            }
            return b;
        }
    }

    [Obsolete("System.Drawing usage")]
    public class DrinksImage : BitmapShape
    {
        public override System.Drawing.Bitmap Render(System.Drawing.Graphics g, System.Drawing.Bitmap b, params int[] options)
        {
            System.Drawing.Color bc = DrawingBackColor;
            System.Drawing.Rectangle r = new System.Drawing.Rectangle() { Height = b.Height, Width = b.Width };
            r.Inflate(-6, -6);
            g.Clear(bc.IsEmpty ? System.Drawing.Color.Transparent : bc);
            g.DrawImage(ResourceLoadHelper.GetImage(ResourceLoadHelper.GetImageName((DrinkType)options[0])), r);
            return b;
        }
    }

    [Obsolete("System.Drawing usage")]
    public class BlankImage : BitmapShape
    {
        public override System.Drawing.Bitmap Render(System.Drawing.Graphics g, System.Drawing.Bitmap b, params int[] options)
        {
            System.Drawing.Color bc = DrawingBackColor;
            g.Clear(bc.IsEmpty ? System.Drawing.Color.Transparent : bc);
            return b;
        }
    }

    [Obsolete("System.Drawing usage")]
    public class CoinCursorImage : BitmapShape
    {
        public override System.Drawing.Bitmap Render(System.Drawing.Graphics g, System.Drawing.Bitmap b, params int[] options)
        {
            System.Drawing.Color bc = DrawingBackColor;
            System.Drawing.Color fc = DrawingForeColor;
            System.Drawing.Rectangle r = new System.Drawing.Rectangle() { Height = b.Height, Width = b.Width };
            r.Inflate(-1, -1);
            g.Clear(System.Drawing.Color.White);
            g.FillEllipse(new System.Drawing.SolidBrush(bc), r);
            g.DrawEllipse(new System.Drawing.Pen(fc), r);
            string s = options[0].ToString();
            if (IsHighDPI)
            {
                r.Inflate(s.Length > 1 ? 1 : -6, -4);
            }
            else
            {
                r.Inflate(s.Length > 1 ? -1 : -7, -5);
            }
            System.Drawing.Font fnt = new System.Drawing.Font("Microsoft Sans Serif", 12, System.Drawing.FontStyle.Bold);
            g.DrawString(s, fnt, new System.Drawing.SolidBrush(fc), r);
            b.MakeTransparent(System.Drawing.Color.White);
            return b;
        }
    }

    [Obsolete("System.Drawing usage")]
    public class PutCoinCursorImage : BitmapShape
    {
        public override System.Drawing.Bitmap Render(System.Drawing.Graphics g, System.Drawing.Bitmap b, params int[] options)
        {
            System.Drawing.Color bc = DrawingBackColor;
            System.Drawing.Color fc = DrawingForeColor;
            System.Drawing.Rectangle r = new System.Drawing.Rectangle() { Height = b.Height, Width = b.Width };
            r.Inflate(-1, -1);
            g.Clear(System.Drawing.Color.White);
            g.FillRectangle(new System.Drawing.SolidBrush(bc), r);
            g.DrawRectangle(new System.Drawing.Pen(fc), r);
            b.MakeTransparent(System.Drawing.Color.White);
            return b;
        }
    }

    public static class ResourceLoadHelper
    {
        private static readonly Dictionary<DrinkType, string> _namePairsCache = new Dictionary<DrinkType, string>();
        private static readonly Dictionary<string, object> _resourcePairsCache = new Dictionary<string, object>();
        private static readonly Dictionary<int, Cursor> _coinCursorCache = new Dictionary<int, Cursor>();
        private static readonly BitmapShape _bImage = new BaseShapeDecorator(new BlankImage());
        private static readonly BitmapShape _cImage = new BaseShapeDecorator(new CoinsImage());
        private static readonly BitmapShape _dImage = new BaseShapeDecorator(new DrinksImage());
        private static readonly BitmapShape _cCursor = new BaseShapeDecorator(new CoinCursorImage());
        private static readonly BitmapShape _pCursor = new BaseShapeDecorator(new PutCoinCursorImage(), 10);

        [Obsolete("System.Drawing usage")]
        public static System.Drawing.Image GetImage(string name)
        {
            if (!_resourcePairsCache.ContainsKey(name))
            {
                object resourceImage = Resources.ResourceManager.GetObject(name);
                _resourcePairsCache.Add(name, (resourceImage != null) && (resourceImage is System.Drawing.Image) ? resourceImage as System.Drawing.Image : null);
            }
            return _resourcePairsCache[name] as System.Drawing.Image;
        }

        public static Image GetBlankImage(Size size, Color backColor)
        {
            _bImage.Size = size;
            _bImage.BackColor = backColor;
            return _bImage.GetImage();
        }

        [Obsolete("System.Drawing usage")]
        public static Image GetCoinsProgressBarLineImage(AccountType type, int coin, Size size, int count, int range, Color backColor, Color foreColor)
        {
            _cImage.Initialize((int)type, coin, count, range);
            _cImage.BackColor = backColor;
            _cImage.ForeColor = foreColor;
            _cImage.Size = size;
            if (count < range)
            {
                System.Drawing.Size isz = _cImage.InnerSize;
                switch (type)
                {
                    case AccountType.Customer:
                        size.Width = Math.Ceiling((double)(count * isz.Width + (range - count) * isz.Height / 3) / range + 1);
                        size.Height = isz.Height;
                        break;
                    case AccountType.VendingMachine:
                        size.Height = Math.Ceiling((double)(count * isz.Height + (range - count) * isz.Width / 3) / range + 1);
                        size.Width = isz.Width;
                        break;
                }
                return _cImage.GetImage(_cImage.Render(size));
            }
            else
            {
                return _cImage.GetImage();
            }
        }

        public static Image GetDrinksButtonImage(Size size, Color backColor, DrinkType type)
        {
            _dImage.Initialize((int)type);
            _dImage.Size = size;
            _dImage.BackColor = backColor;
            return _dImage.GetImage();
        }

        public static Cursor GetCoinCursor(Color fillColor, Color drawColor, int coin)
        {
            if (!_coinCursorCache.ContainsKey(coin))
            {
                _cCursor.Initialize(coin);
                _cCursor.BackColor = fillColor;
                _cCursor.ForeColor = drawColor;
                _coinCursorCache[coin] = _cCursor.GetCursor();
            }
            return _coinCursorCache[coin];
        }

        public static Cursor GetPutCoinCursor(Color fillColor, Color drawColor)
        {
            if (!_coinCursorCache.ContainsKey(0))
            {
                _pCursor.Initialize();
                _pCursor.BackColor = fillColor;
                _pCursor.ForeColor = drawColor;
                _coinCursorCache[0] = _pCursor.GetCursor();
            }
            return _coinCursorCache[0];
        }

        public static string GetImageName(DrinkType kind)
        {
            if (!_namePairsCache.ContainsKey(kind))
            {
                object resourceImage = Resources.ResourceManager.GetObject(kind == DrinkType.Tea ? "TeaImageName" : kind == DrinkType.Coffee ? "CoffeeImageName" : kind == DrinkType.Juice ? "JuiceImageName" : "ProgressImageName");
                _namePairsCache.Add(kind, (resourceImage != null) && (resourceImage is string) ? resourceImage as string : string.Empty);
            }
            return _namePairsCache[kind] as string;
        }

        public static string GetLocalString(string name)
        {
            if (!_resourcePairsCache.ContainsKey(name))
            {
                object resourceImage = Resources.ResourceManager.GetObject(string.Format("_{0}_{1}", Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, name));
                _resourcePairsCache.Add(name, (resourceImage != null) && (resourceImage is string) ? resourceImage as string : string.Empty);
            }
            return _resourcePairsCache[name] as string;
        }

        public static string GetLocalName(Dictionary<string, string> locals, string localization = null)
        {
            string local = localization ?? Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            return locals?.ContainsKey(local) ?? false ? locals[local] : null;
        }
    }
}
