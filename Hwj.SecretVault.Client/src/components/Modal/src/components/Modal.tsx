/*
 * @Author: git config user.name && git config user.email
 * @Date: 2023-06-05 11:39:17
 * @LastEditors: git config user.name && git config user.email
 * @LastEditTime: 2023-06-05 11:43:56
 * @FilePath: \CastcZhy.Tmp.WebClient.VbenAdmin\src\components\Modal\src\components\Modal.tsx
 * @description: 描述:
 */
import { Modal } from 'ant-design-vue'
import { defineComponent, toRefs, unref } from 'vue'
import { basicProps } from '../props'
import { useModalDragMove } from '../hooks/useModalDrag'
import { useAttrs } from '@/hooks/core/useAttrs'
import { extendSlots } from '@/utils/helper/tsxHelper'


export default defineComponent({
	name: 'Modal',
	inheritAttrs: false,
	props: basicProps as any,
	emits: ['cancel'],
	setup(props, { slots, emit }) {
		const { visible, draggable, destroyOnClose } = toRefs(props)
		const attrs = useAttrs()
		useModalDragMove({
			visible,
			destroyOnClose,
			draggable
		})

		const onCancel = (e: Event) => {
			emit('cancel', e)
		}

		return () => {
			const propsData = { ...unref(attrs), ...props, onCancel } as Recordable
			return <Modal {...propsData}>{extendSlots(slots)}</Modal>
		}
	}
})
