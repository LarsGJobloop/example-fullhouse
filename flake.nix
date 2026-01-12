{
  description = "FullHouse";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs?ref=nixos-unstable";
  };

  outputs =
    { self, nixpkgs }:
    let
      supportedSystems = [
        "x86_64-linux"
        "x86_64-darwin" # TODO! NOT Verified
        "aarch64-linux" # TODO! NOT Verified
        "aarch64-darwin"
      ];
      withSystem = nixpkgs.lib.genAttrs supportedSystems;
      withPkgs = callback: withSystem (system: callback (import nixpkgs { inherit system; }));
    in
    {
      devShells = withPkgs (pkgs: {
        default = pkgs.mkShell {
          packages = with pkgs; [
            sops
            age
            opentofu
            dotnetCorePackages.sdk_10_0
          ];

          env = {
            DOTNET_ROOT = "${pkgs.dotnetCorePackages.sdk_10_0}/lib/dotnet";
          };
        };
      });

      formatter = withPkgs (pkgs: pkgs.nixfmt-rfc-style);
    };
}
