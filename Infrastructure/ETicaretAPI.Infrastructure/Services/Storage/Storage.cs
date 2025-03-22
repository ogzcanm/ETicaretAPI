using ETicaretAPI.Infrastructure.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Storage
{
    // Storage sınıfı, dosya ismi değiştirme ve kontrol etme gibi işlemleri barındıran sınıf
    public class Storage
    {
        // HasFile delegate'inin tanımlanması: Bu delegate, dosyanın var olup olmadığını kontrol eden metodu temsil eder
        protected delegate bool HasFile(string pathOrContainerName, string fileName);

        // FileRenameAsync fonksiyonu: Bu asenkron fonksiyon, dosya ismini değiştirme işlemi yapar
        protected async Task<string> FileRenameAsync(string pathOrContainerName, HasFile hasFileMethod, string fileName, bool first = true)
        {
            // Asenkron olarak dosya adı değişikliği yapılacak işlem başlatılıyor
            string newFileName = await Task.Run(async () =>
            {
                // Dosyanın uzantısını alıyoruz
                string extension = Path.GetExtension(fileName);
                string newFileName = string.Empty;  // Yeni dosya ismi

                if (first)  // Eğer bu ilk defa dosya ismi değiştiriliyorsa
                {
                    // Dosyanın uzantısı dışında kalan ismini alıyoruz
                    string oldName = Path.GetFileNameWithoutExtension(fileName);
                    // Yeni dosya adını, karakter düzenlemesi yaparak belirliyoruz
                    newFileName = $"{NameOperation.CharacterRegulatory(oldName)}{extension}";
                }
                else  // Eğer dosya ismi birden fazla kez değiştirilmişse
                {
                    newFileName = fileName;  // Dosya adı mevcut haliyle alınıyor
                    int indexNo1 = newFileName.IndexOf("-");  // Dosya adında "-" karakteri aranıyor

                    if (indexNo1 == -1)  // Eğer "-" karakteri yoksa, dosya adına "-2" ekleniyor
                    {
                        newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                    }
                    else  // Eğer "-" varsa, dosya adının sonundaki sayıyı arttırıyoruz
                    {
                        int lastIndex = 0;
                        while (true)
                        {
                            lastIndex = indexNo1;
                            indexNo1 = newFileName.IndexOf("-", indexNo1 + 1);  // Bir sonraki "-" karakterini arıyoruz
                            if (indexNo1 == -1)
                            {
                                indexNo1 = lastIndex;
                                break;
                            }
                        }

                        // Dosya adındaki sayıyı almak için nokta öncesindeki kısmı çıkarıyoruz
                        int indexNo2 = newFileName.IndexOf(".");
                        string fileNo = newFileName.Substring(indexNo1 + 1, indexNo2 - indexNo1 - 1);

                        // Eğer bu sayıyı sayısal olarak parse edebiliyorsak, sayıyı arttırıyoruz
                        if (int.TryParse(fileNo, out int _fileNo))
                        {
                            _fileNo++;  // Sayıyı bir artırıyoruz
                            newFileName = newFileName.Remove(indexNo1 + 1, indexNo2 - indexNo1 - 1).Insert(indexNo1 + 1, _fileNo.ToString());
                        }
                        else  // Eğer sayıyı arttıramıyorsak, dosya adına "-2" ekliyoruz
                        {
                            newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                        }
                    }
                }

                // Dosya sisteminde bu ismin olup olmadığını kontrol ediyoruz
                if (hasFileMethod(pathOrContainerName, newFileName))
                {
                    // Eğer dosya zaten varsa, yeniden isim değiştiriyoruz
                    return await FileRenameAsync(pathOrContainerName, hasFileMethod, newFileName, false);
                }
                else
                {
                    // Eğer dosya yoksa, yeni dosya ismini döndürüyoruz
                    return newFileName;
                }
            });

            // Sonuç olarak bulunan yeni dosya adını döndürüyoruz
            return newFileName;
        }
    }
}
