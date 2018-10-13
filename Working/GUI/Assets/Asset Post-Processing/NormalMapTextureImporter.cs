using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN asset_postprocessor
#if UNITY_EDITOR
using UnityEditor;

public class NormalMapTextureImporter : AssetPostprocessor
{

    // Called _before_ the texture is imported. 
    void OnPreprocessTexture() {

        // Get the name of the file.
        var filename = 
            System.IO.Path.GetFileNameWithoutExtension(assetPath);

        // We're looking for texture files that end in any of these
        // suffixes.
        var normalMapSuffixes = new[] { "_n", "_normal", "_nrm" };

        // Check each one
        foreach (var suffix in normalMapSuffixes) {
            if (filename.EndsWith(suffix)) {

                // Get the texture importer that's currently importing this
                // texture
                TextureImporter textureImporter = 
                    assetImporter as TextureImporter;

                // Update its type so that Unity is aware that it's a
                // normal map
                textureImporter.textureType = 
                    TextureImporterType.NormalMap;

                // Exit here, since we know we don't need to do any more
                // work
                return;
            }
        }
    }



}
#endif
// END asset_postprocessor