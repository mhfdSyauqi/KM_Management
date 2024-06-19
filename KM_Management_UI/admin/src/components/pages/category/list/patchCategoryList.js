import { PatchCategoryListAsync} from '@/api/reqCategory.js'
import { ref } from 'vue'

const editCategory = ref({
    Uid:'',
    Name: '',
    Is_Active: '',
})

const errorEdit = ref({
  name: { isError: false, message: '' },
})


async function HandleRePublish() {

  const patchCategory = await PatchCategoryListAsync(
    editCategory.value.Uid,
    editCategory.value.Name,
    editCategory.value.Is_Active,
  )

  if (!patchCategory.is_success) {
    if (patchCategory.error.statuscode === 400) {
      errorEdit.value.name.isError = true
      errorEdit.value.name.message = patchCategory.error.message
    }
    return patchCategory.error.statuscode
  }

  ResetEditInput()
  return true
}

function ResetEditInput() {
  for (const key in editCategory.value) {
    editCategory.value[key] = ''
  }

  for (const key in errorEdit.value) {
    errorEdit.value[key].isError = false
    errorEdit.value[key].message = ''
  }
}

export { editCategory, errorEdit, HandleRePublish, ResetEditInput }