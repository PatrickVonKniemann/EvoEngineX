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
                    {include: '@whitespace'},
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

        // Create the Monaco Editor instance
        window.monacoEditor = monaco.editor.create(element, options);

        window.monacoEditor.onDidChangeModelContent(() => {
            let code = window.monacoEditor.getValue();
            console.log('Editor content changed:', code);
            window.dotNetHelper.invokeMethodAsync('UpdateCode', code)
                .then(() => console.log('UpdateCode invoked successfully'))
                .catch(err => console.error('Error invoking UpdateCode:', err));
        });
    });

};

window.setMonacoEditorValue = (formattedCode) => {
    if (window.monacoEditor) {
        window.monacoEditor.setValue(formattedCode);
    }
};

window.getMonacoEditorValue = () => {
    if (window.monacoEditor) {
        return window.monacoEditor.getValue();
    }
    return '';
};

window.highlightWords = (words) => {
    console.log("Highlight words");
    if (window.monacoEditor) {
        console.log(`monacoEditor is setup search sing texts:`);

        const model = window.monacoEditor.getModel();
        const fullText = model.getValue();
        console.log(fullText);

        // Clear existing decorations
        window.decorations = window.monacoEditor.deltaDecorations(window.decorations || [], []);

        // Create new decorations
        const newDecorations = [];

        words.forEach((word) => {
            let matchIndex = fullText.indexOf(word);
            console.log(`Searching for word: ${word}`);
            while (matchIndex !== -1) {
                console.log(`Word '${word}' found at index: ${matchIndex}`);

                const startPosition = model.getPositionAt(matchIndex);
                const endPosition = model.getPositionAt(matchIndex + word.length);

                newDecorations.push({
                    range: new monaco.Range(startPosition.lineNumber, startPosition.column, endPosition.lineNumber, endPosition.column),
                    options: {
                        inlineClassName: 'myInlineDecoration',
                        overviewRuler: {
                            position: monaco.editor.OverviewRulerLane.Right
                        }
                    }
                });

                matchIndex = fullText.indexOf(word, matchIndex + word.length);
            }
        });

        // Apply new decorations
        window.decorations = window.monacoEditor.deltaDecorations([], newDecorations);
    }
};