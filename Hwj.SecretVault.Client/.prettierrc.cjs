module.exports = {
  semi: false,
  // vue文件中script和style中代码是否缩进
  vueIndentScriptAndStyle: true,
  singleQuote: true,
  trailingComma: 'none',
  proseWrap: 'never',
  htmlWhitespaceSensitivity: 'strict',
  endOfLine: 'crlf',
  plugins: ['prettier-plugin-packagejson'],
  overrides: [
    {
      files: '.*rc',
      options: {
        parser: 'json',
      },
    },
  ],
};
