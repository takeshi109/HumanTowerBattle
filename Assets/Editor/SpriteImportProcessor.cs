using UnityEditor;
using UnityEngine;

public class SpriteImportProcessor : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        TextureImporter importer = (TextureImporter)assetImporter;

        // Resourcesフォルダ内の画像のみ対象
        if (importer.assetPath.Contains("Resources"))
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single; // 自動でSingleに設定
        }
    }
}