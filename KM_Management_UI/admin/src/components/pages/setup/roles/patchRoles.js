import { ref } from 'vue'
import { PatchUserRoleAsync } from '@/api/reqRole.js'

const editedUser = ref({
  user_name: '',
  full_name: '',
  role: ''
})

async function HandleEdit() {
  const patchRole = await PatchUserRoleAsync(editedUser.value.user_name, editedUser.value.role)

  return patchRole.is_success
}

export { editedUser, HandleEdit }