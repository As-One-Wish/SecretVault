import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import { resolve } from 'path'
// 指定解析路径

const pathResolve = (dir: string) => resolve(__dirname, dir)

export default defineConfig({
	plugins: [vue()],
	base: '/',
	resolve: {
		// 路径别名
		alias: [
			{ find: '@', replacement: pathResolve('src') },
			{ find: '#', replacement: pathResolve('types') },
			{ find: 'api', replacement: pathResolve('src/api') },
			{ find: 'components', replacement: pathResolve('src/components') },
			{ find: 'utils', replacement: pathResolve('src/utils') },
			// 处理 vue-i18n 的控制台警告信息
			{
				find: 'vue-i18n',
				replacement: 'vue-i18n/dist/vue-i18n.cjs.js'
			}
		]
	}
})
