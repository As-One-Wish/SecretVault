import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import Components from 'unplugin-vue-components/vite'
import { AntDesignVueResolver } from 'unplugin-vue-components/resolvers'
import { resolve } from 'path'

// https://vitejs.dev/config/
export default defineConfig({
	plugins: [
		vue(),
		Components({
			resolvers: [AntDesignVueResolver({ importStyle: 'less' })]
		})
	],
	resolve: {
		alias: {
			'@': resolve(__dirname, 'src'),
			'#': resolve(__dirname, 'types')
		}
	}
})
