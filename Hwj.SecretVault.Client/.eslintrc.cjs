module.exports = {
	'env': {
		'es2021': true,
		'node': true
	},
	'extends': [
		'eslint:recommended',
		'plugin:@typescript-eslint/recommended',
		'plugin:vue/vue3-essential'
	],
	'overrides': [
		{
			'env': {
				'node': true
			},
			'files': [
				'.eslintrc.{js,cjs}'
			],
			'parserOptions': {
				'sourceType': 'script'
			}
		}
	],
	'parserOptions': {
		'ecmaVersion': 'latest',
		'parser': '@typescript-eslint/parser',
		'sourceType': 'module'
	},
	'plugins': [
		'@typescript-eslint',
		'vue'
	],
	'rules': {
		// 缩进类型
		'indent': ['error', 'tab'],
		// 行结束符类型 windows-CRLF
		'linebreak-style': ['error', 'windows'],
		// 引号类型
		'quotes': ['error', 'single'],
		// 结尾分号
		'semi': ['error','never'],
		// 数组和对象键值对最后一个逗号
		'comma-dangle': ['error', 'never'],
		// 数组对象之间的空格
		'array-bracket-spacing': ['error', 'never'],
		// 中缀操作符之间是否需要空格
		'space-infix-ops': ['error'],
		// 对象字面量中冒号的前后空格
		'key-spacing': ['error', { 'beforeColon': false, 'afterColon': true }],
		// clear eslintvue/comment-directive 错误解决
		'vue/comment-directive': 'off',
		// 设置html缩进为两个
		'vue/html-indent': ['error', 'tab'],
		// 配置ts参数声明类型格式冒号前后空格
		'@typescript-eslint/type-annotation-spacing': ['error', {
			'before': false,
			'after': true
		}],
		// 不能有多余的空格
		'no-multi-spaces': ['error'],
		// 不能有不规则的空格
		'no-irregular-whitespace': ['error'],
		//生成器函数*的前后空格
		'generator-star-spacing': ['error']
	}
}

