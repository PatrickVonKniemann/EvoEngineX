window.setDotNetHelper = (dotNetHelper) => {
    window.dotNetHelper = dotNetHelper;
};

window.createMonacoEditor = (element, options) => {
    require.config({paths: {'vs': 'https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.21.2/min/vs'}});
    require(['vs/editor/editor.main'], function () {
        // Register the MATLAB language
        monaco.languages.register({id: 'matlab'});

        monaco.languages.setMonarchTokensProvider('matlab', {
            defaultToken: '',
            tokenPostfix: '.m',
            keywords: [
                'break', 'case', 'catch', 'classdef', 'continue', 'else', 'elseif', 'end', 'for', 'function',
                'global', 'if', 'otherwise', 'parfor', 'persistent', 'return', 'spmd', 'switch', 'try', 'while'
            ],
            symbols: /[=><!~?:&|+\-*\/\^%]+/,
            escapes: /\\(?:[abfnrtv\\"']|x[0-9A-Fa-f]{1,4}|u[0-9A-Fa-f]{4})/,
            tokenizer: {
                root: [
                    [/[a-zA-Z_$][\w$]*/, {
                        cases: {
                            '@keywords': 'keyword',
                            '@default': 'identifier'
                        }
                    }],
                    { include: '@whitespace' },
                    [/[{}()\[\]]/, '@brackets'],
                    [/[<>](?!@symbols)/, '@brackets'],
                    [/[;,.]/, 'delimiter'],
                    [/"([^"\\]|\\.)*$/, 'string.invalid'],  // non-terminated string
                    [/"/, 'string', '@string'],
                    [/\d*\.\d+([eE][-+]?\d+)?/, 'number.float'],
                    [/\d+/, 'number'],
                ],
                string: [
                    [/[^\\"]+/, 'string'],
                    [/@escapes/, 'string.escape'],
                    [/\\./, 'string.escape.invalid'],
                    [/"/, 'string', '@pop']
                ],
                whitespace: [
                    [/[ \t\r\n]+/, ''],
                    [/^\s*#.*/, 'comment'],
                ],
            }
        });
    });
};
