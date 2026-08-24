using System.Reflection;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Report.Pdf.Fonts;

public class ExpensesReportFontsResolver : IFontResolver
{
   public byte[] GetFont(string faceName)
   {
        var fontStream = ReadFontFile(faceName);

        fontStream ??= ReadFontFile(FontHelper.DEFAULT_FONT);
        
        var length = (int)fontStream!.Length;

        var data = new byte[length];
        
        fontStream.Read(buffer: data, offset: 0, count: (int)length);

        return data;
   }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        return new FontResolverInfo(familyName);
    }

    private Stream? ReadFontFile(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"CashFlow.Application.UseCases.Expenses.Report.Pdf.Fonts.{faceName}.ttf";
        return assembly.GetManifestResourceStream(resourceName);
    }
  
}