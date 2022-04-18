using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace LearnOpenTK.Common
{
    // A helper class, much like Shader, meant to simplify loading textures.
    public class Texture
    {
        public readonly int Handle;

        public static Texture LoadFromFile(string path)
        {
            // Главный дискриптор
            int handle = GL.GenTexture();

            // Связаться с этим дискриптором
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);

            // Для этого примера мы собираемся использовать .Встроенная система NET.Библиотека рисования для загрузки текстур.

            // Загрузить изображение

            using (var image = new Bitmap(path))
            {
                // Наше растровое изображение загружается из верхнего левого пикселя, в то время как OpenGL загружается из нижнего левого, в результате чего текстура переворачивается по вертикали.
                // Это исправит это, заставив текстуру отображаться правильно.
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                // Во-первых, мы получаем наши пиксели из загруженного нами растрового изображения.
                // Аргументы:
                // Область пикселей, которую мы хотим. Обычно вы хотите оставить его равным (0,0) или (ширина, высота), но вы можете
                // // использовать другие прямоугольники для получения сегментов текстур, полезных для таких вещей, как листы спрайтов.
                // Режим блокировки. В принципе, как вы хотите использовать пиксели. Поскольку мы передаем их в OpenGL,
                // // нам нужно только чтение.
                // Далее идет формат пикселей, в котором мы хотим, чтобы были наши пиксели. В этом случае будет достаточно ARGB.
                // // Мы должны полностью указать имя, потому что OpenTK также имеет перечисление с именем Pixel Format.
                var data = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                // Теперь, когда наши пиксели подготовлены, пришло время создать текстуру. Мы делаем это с помощью GL.texImage2D.
                // Аргументы:
                // Тип текстуры, которую мы создаем. Существуют различные типы текстур, но единственная, которая нам сейчас нужна, - это Texture2D.
                // Уровень детализации. Мы можем использовать это, чтобы начать с меньшего mipmap (если захотим), но нам не нужно этого делать, поэтому оставьте его равным 0.
                // Целевой формат пикселей. Это формат, в котором OpenGL будет хранить наше изображение.
                // Ширина изображения
                // Высота изображения.
                // Граница изображения. Это всегда должно быть равно 0; это устаревший параметр, от которого Хронос так и не избавился.
                // Формат пикселей, описанный выше. Поскольку ранее мы загрузили пиксели как ARGB, нам нужно использовать BGRA.
                // Тип данных пикселей.
                // И, наконец, фактические пиксели.
                GL.TexImage2D(TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.Rgba,
                    image.Width,
                    image.Height,
                    0,
                    PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    data.Scan0);
            }

            // Теперь, когда наша текстура загружена, мы можем установить несколько настроек, чтобы повлиять на то, как изображение отображается при рендеринге.

            // // Сначала мы устанавливаем минимальный и максимальный фильтры. Они используются, когда текстура масштабируется соответственно вниз и вверх.
            // Здесь мы используем Linear для обоих. Это означает, что OpenGL попытается смешать пиксели, а это означает, что текстуры, масштабированные слишком далеко, будут выглядеть размытыми.
            // Вы также можете использовать (среди других вариантов) Ближайший, который просто захватывает ближайший пиксель, из-за чего текстура выглядит пикселизированной, если масштабировать ее слишком сильно.
            // // ПРИМЕЧАНИЕ: Настройки по умолчанию для обоих из них - Линейная Mipmap. Если вы оставите их по умолчанию, но не создадите mipmaps,
            // ваше изображение вообще не будет отображаться (обычно вместо этого получается чистый черный цвет).
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Теперь установите режим обертывания. S - для оси X, а T - для оси Y.
            // Мы устанавливаем для этого значение Repeat, чтобы текстуры повторялись при обертывании. Здесь не показано, так как координаты текстуры точно совпадают
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            // Далее, generer// Wheremipmaps.
            // Mipmapsmovies - Уменьшенные копии текстуры в уменьшенном масштабе. Каждый уровень mipmap имеет половину размера предыдущего
            // Сгенерированные mipmaps уменьшаются всего до одного пикселя.
            // OpenGL автоматически переключается между mip-картами, когда объект находится достаточно далеко.
            // Это предотвращает муаровые эффекты, а также экономит пропускную способность текстуры.
            // Здесь вы можете увидеть и прочитать об эффекте Морье https://en.wikipedia.org/wiki/Moir%C3%A9_pattern
            // Вот пример mips в действии https://en.wikipedia.org/wiki/File:Mipmap_Aliasing_Comparison.png
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new Texture(handle);
        }

        public Texture(int glHandle)
        {
            Handle = glHandle;
        }

        // Активировать текстуру
        // Можно связать несколько текстур, если вашему шейдеру нужно больше, чем одна.
        // Если вы хотите это сделать, используйте GL.ActiveTexture, чтобы установить, к какому слоту привязывается GL.bindTexture.
        // Стандарт OpenGL требует, чтобы их было не менее 16, но в зависимости от вашей видеокарты их может быть больше.
        public void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}
