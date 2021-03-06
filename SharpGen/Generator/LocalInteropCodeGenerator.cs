﻿using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using SharpGen.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SharpGen.Generator
{
    class LocalInteropCodeGenerator : ICodeGenerator<CsAssembly, NamespaceDeclarationSyntax>
    {
        public LocalInteropCodeGenerator(IGeneratorRegistry generators)
        {
            Generators = generators;
        }

        public IGeneratorRegistry Generators { get; }

        public NamespaceDeclarationSyntax GenerateCode(CsAssembly csElement)
        {
            return NamespaceDeclaration(ParseName(csElement.QualifiedName))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        ClassDeclaration(Identifier("LocalInterop"))
                            .WithModifiers(TokenList(
                                Token(SyntaxKind.InternalKeyword),
                                Token(SyntaxKind.StaticKeyword),
                                Token(SyntaxKind.PartialKeyword)))
                            .WithMembers(List(csElement.Interop.Methods.SelectMany(sig => Generators.InteropMethod.GenerateCode(sig))))));
        }
    }
}
