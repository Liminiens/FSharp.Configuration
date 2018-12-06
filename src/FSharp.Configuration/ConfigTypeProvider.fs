module FSharp.Configuration.ConfigTypeProvider

open FSharp.Configuration.Helper
open Microsoft.FSharp.Core.CompilerServices
open ProviderImplementation.ProvidedTypes
open System.Text

[<TypeProvider>]
type FSharpConfigurationProvider(cfg: TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces(cfg, addDefaultProbingLocation = true)
    #if !NET45
    static do 
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
    #endif
    let context = new Context(this, cfg)
    do this.AddNamespace (
            rootNamespace,
            [
              ResXProvider.typedResources context
              AppSettingsTypeProvider.typedAppSettings context
              YamlConfigTypeProvider.typedYamlConfig context
              IniFileProvider.typedIniFile context ])
    do this.Disposing.Add (fun _ -> dispose context)

[<TypeProviderAssembly>]
do ()
