import { defHttp } from '@/utils/http/axios'
import { DemoParams, DemoListGetResultModel } from './model/tableModel'

enum Api {
  DEMO_LIST = '/table/getDemoList',
}

/**
 * @description: Get sample list value
 */

export const demoListApi = (params: DemoParams) =>
	defHttp.get<DemoListGetResultModel>({
		url: Api.DEMO_LIST,
		params,
		headers: {
			// eslint-disable-next-line @typescript-eslint/ban-ts-comment
			// @ts-ignore
			ignoreCancelToken: true
		}
	})
