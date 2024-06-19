import { PostCategoryListAsync} from '@/api/reqCategory.js'
import { ref } from 'vue'

const newCategory = ref({
    Name: '',
    Layer: '',
    Is_Active: '',
    Uid_Reference: '',
})

const errorInput = ref({
  name: { isError: false, message: '' },
})


async function HandlePublish() {

  const postCategory = await PostCategoryListAsync(
    newCategory.value.Name,
    newCategory.value.Layer,
    newCategory.value.Is_Active,
    newCategory.value.Uid_Reference
  )

  if (!postCategory.is_success) {
    if (postCategory.error.statuscode === 400) {
      errorInput.value.name.isError = true
      errorInput.value.name.message = postCategory.error.message
    }
    return postCategory.error.statuscode
  }

  ResetPostInput()
  return true
}

function ResetPostInput() {
  for (const key in newCategory.value) {
    newCategory.value[key] = ''
  }

  for (const key in errorInput.value) {
    errorInput.value[key].isError = false
    errorInput.value[key].message = ''
  }
}

export { newCategory, errorInput, HandlePublish, ResetPostInput }