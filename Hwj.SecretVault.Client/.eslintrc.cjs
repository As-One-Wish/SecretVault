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
		// 行结束符类型 unix-LF
		'linebreak-style': ['error', 'unix'],
		// 引号类型
		'quotes': ['error', 'single'],
		// 结尾分号
		'semi': ['error','never'],
		// 数组和对象键值对最后一个逗号
		'comma-dangle': ['error', 'never'],
		// 数组对象之间的空格
		'array-bracket-spacing': ['error', 'never'],
		// 中缀操作符之间是否需要空格
		'space-infix-ops': 'Error',
		// 对象字面量中冒号的前后空格
		'key-spacing': [0, { 'beforeColon': false, 'afterColon': true }]
	}
}
