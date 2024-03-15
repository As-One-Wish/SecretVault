/**
 * @description: 对应后台的BaseParam类
 */
export interface BaseParam{
  top?: number,
  pageSize?: number,
  pageIndex?: number,
  orderBy?: string,
  orderByType: string
}
/**
 * @description: 对应后台的BaseResult 单对象
 */
export interface BaseResult<T = any>{
  isSuccess: boolean,
  data?: T,
  message: string,
  dataCount?: number
}