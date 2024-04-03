import { DeleteUserRoleAsync } from '@/api/reqRole.js'
import { ConfirmSwal, ToastSwal } from '@/extension/SwalExt.js'
import { useNotificationStore } from '@/stores/useNotification.js'

async function HandleDelete(user) {
  const { isConfirmed } = await ConfirmSwal.fire({
    text: `want to delete ${user.full_name} from list`
  })

  if (!isConfirmed) return

  const notificationStore = useNotificationStore()
  const patchRole = await DeleteUserRoleAsync(user.login_name)

  if (!patchRole.is_success)
    return await ToastSwal.fire({
      icon: 'error',
      text: "can't connect to server, please try in a few minutes"
    })

  notificationStore.set('reload')
  return await ToastSwal.fire({
    icon: 'success',
    text: 'success delete account'
  })
}

export { HandleDelete }